%addpath('quaternion_library');

filename = 'Swing 23D 15 sec.txt';
delimiterIn = '\t';
headerlinesIn = 1;
A = importdata(filename, delimiterIn, headerlinesIn);
acceleration(:,1)=(A.data(:,1))./4096;
acceleration(:,2)=(A.data(:,2))./4096;
acceleration(:,3)=(A.data(:,3))./4096;
velocity(:,1)=(A.data(:,4))./(32.75);
velocity(:,2)=(A.data(:,5))./(32.75);
velocity(:,3)=(A.data(:,6))./(32.75);
mag(:,1)=(A.data(:,7));
mag(:,2)=(A.data(:,8));
mag(:,3)=(A.data(:,9));
Ltime(1,:) = (A.data(:,13))./1000;      %convert to milliseconds

T = (Ltime(end)-Ltime(1))/length(Ltime);     %find average period
t = (1:length(Ltime)).*T;       %create time vector based on average period

aa = velocity.*T;     %axis angle

for i=1:length(aa)
    angle(i,:,1) = norm(aa(i,:));
    aa(i,:) = aa(i,:)./norm(aa(i,:));
    acceleration(i,:) = acceleration(i,:)./norm(acceleration(i,:));
    mag(i,:) = mag(i,:)./norm(mag(i,:));
end

q(:,1) = cosd(angle./2);     %quaternion
q(:,2) = aa(:,1).*sind(angle./2);
q(:,3) = aa(:,2).*sind(angle./2);
q(:,4) = aa(:,3).*sind(angle./2);

%{
subplot(3,1,1)
hold on
plot (t, velocity(:,1), 'r')
plot (t, velocity(:,2), 'g')
plot (t, velocity(:,3), 'b')
hold off

subplot(3,1,2)
hold on
plot (t, mag(:,1), 'r')
plot (t, mag(:,2), 'g')
plot (t, mag(:,3), 'b')
hold off
legend('x', 'y', 'z')

subplot(3,1,3)
hold on
plot (t, acceleration(:,1), 'r')
plot (t, acceleration(:,2), 'g')
plot (t, acceleration(:,3), 'b')
hold off
legend('x', 'y', 'z')
%}

point(1,:) = [0 acceleration(1,:)];
for i=2:length(q)
    point(i,:) = quaternProd(q(i,:),quaternProd(point(i-1,:),quaternConj(q(1,:))));
end;

euler = quatern2euler(quaternConj(point));
figure('Name', 'Euler Angles');
hold on;
plot(t, euler(:,1), 'r');
plot(t, euler(:,2), 'g');
plot(t, euler(:,3), 'b');
title('Euler angles');
xlabel('Time (s)');
ylabel('Angle (deg)');
legend('\phi', '\theta', '\psi');
hold off;
