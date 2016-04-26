%addpath('quaternion_library');

filename = 'Swing 23D 15 sec.txt';
delimiterIn = '\t';
headerlinesIn = 1;
A = importdata(filename, delimiterIn, headerlinesIn);
acc(:,1)=(A.data(:,1))./4096;
acc(:,2)=(A.data(:,2))./4096;
acc(:,3)=(A.data(:,3))./4096;
gyro(:,1)=(A.data(:,4))./(32.75);
gyro(:,2)=(A.data(:,5))./(32.75);
gyro(:,3)=(A.data(:,6))./(32.75);
mag(:,1)=(A.data(:,7));
mag(:,2)=(A.data(:,8));
mag(:,3)=(A.data(:,9));
Ltime(1,:) = (A.data(:,13))./1000;      %convert to milliseconds

%*******low pass filter*****
x_filter = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',10e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);
gyro = filter(x_filter,gyro);

%correction factor (eliminate offset "constant")
mean_x = mean(gyro(:,1));
mean_y = mean(gyro(:,2));
mean_z = mean(gyro(:,3));
gyro(:,1)=gyro(:,1)-mean_x;
gyro(:,2)=gyro(:,2)-mean_y;
gyro(:,3)=gyro(:,3)-mean_z;

T = (Ltime(end)-Ltime(1))/length(Ltime);     %find average period
t = (1:length(Ltime)).*T;       %create time vector based on average period

aa = gyro.*T;     %create axis matrix

%This is an example addition

for i=1:length(aa)  %create angle matrix
    angle(i,:,1) = norm(aa(i,:));
    aa(i,:) = aa(i,:)./norm(aa(i,:));
    acc(i,:) = acc(i,:)./norm(acc(i,:));
    mag(i,:) = mag(i,:)./norm(mag(i,:));
end

q(:,1) = cosd(angle./2);     %create quaternion matrix
q(:,2) = aa(:,1).*sind(angle./2);
q(:,3) = aa(:,2).*sind(angle./2);
q(:,4) = aa(:,3).*sind(angle./2);

qpoint(1,:) = [0 acc(1,:)];
for i=2:length(q)
    qpoint(i,:) = quaternProd(q(i,:),quaternProd(qpoint(i-1,:),quaternConj(q(1,:))));
end;

euler = (quatern2euler(quaternConj(qpoint))).*(180/pi);

%subplot(2,1,1);
hold on;
plot(t, euler(:,1), 'r');
plot(t, euler(:,2), 'g');
plot(t, euler(:,3), 'b');
title('Euler angles');
xlabel('Time (s)');
ylabel('Angle (deg)');
legend('\phi', '\theta', '\psi');
hold off;

%{
x=zeros(size(gyro));
subplot(2,1,2);
quiver3(x(:,1),x(:,2),x(:,3),q(:,2),q(:,3),q(:,4));
%xlim([-1 1]), ylim([-1 1]), zlim([-1 1]);
xlabel('x'), ylabel('y'), zlabel('z')
%}