filenameS1 = 'ViconTest11.txt';
filenameS2 = 'ViconTest12.txt';
delimiterIn = ' ';
headerlinesIn = 1;
S1 = importdata(filenameS1, delimiterIn, headerlinesIn);
S2 = importdata(filenameS2, delimiterIn, headerlinesIn);

GyroS1(:,1) = (S1.data(:,4))./32.75;
GyroS1(:,2) = (S1.data(:,5))./32.75;
GyroS1(:,3) = (S1.data(:,6))./32.75;
GyroS2(:,1) = (S2.data(:,4))./32.75;
GyroS2(:,2) = (S2.data(:,5))./32.75;
GyroS2(:,3) = (S2.data(:,6))./32.75;
kS1 = S1.data(:,10);
kS2 = S2.data(:,10);
TestDurationS1 = kS1-kS1(1);
TestDurationS1 = transpose(TestDurationS1./1000);
TestDurationS2 = kS2-kS2(1);
TestDurationS2 = transpose(TestDurationS2./1000);

%*******low pass filter*****
x_filter = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',10e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);
GyroS1 = filter(x_filter,GyroS1);
GyroS2 = filter(x_filter,GyroS2);

%best fit code
tS1(:,1) = transpose(TestDurationS1);
tS1(:,2) = transpose(TestDurationS1);
tS1(:,3) = transpose(TestDurationS1);
p1 = polyfit(tS1,GyroS1,1);
c_velocityS1 = transpose(polyval(p1,TestDurationS1));

tS2(:,1) = transpose(TestDurationS2);
tS2(:,2) = transpose(TestDurationS2);
tS2(:,3) = transpose(TestDurationS2);
p2 = polyfit(tS2,GyroS2,1);
c_velocityS2 = transpose(polyval(p2,TestDurationS2));

%differentiation and integration
a_accelerationS1 = diff(GyroS1);             % vel to accel 
a_accelerationS1 = [0,[1 3];a_accelerationS1];
a_jerkS1 = diff(a_accelerationS1);             %accel to jerk
a_jerkS1 = [0,[1 3];a_jerkS1];
a_positionS1 = trapz(TestDurationS1,GyroS1);
a_distanceS1 = cumtrapz(TestDurationS1,GyroS1);     % vel to distance

a_accelerationS2 = diff(GyroS2);             % vel to accel 
a_accelerationS2 = [0,[1 3];a_accelerationS2];
a_jerkS2 = diff(a_accelerationS2);             %accel to jerk
a_jerkS2 = [0,[1 3];a_jerkS1];
a_positionS2 = trapz(TestDurationS2,GyroS2);
a_distanceS2 = cumtrapz(TestDurationS2,GyroS2);     % vel to distance

%angular distance to linear position
positionS1(:,1) = -1+cosd(a_distanceS1(:,3))+sind(a_distanceS1(:,2));
positionS1(:,2) = -1+cosd(a_distanceS1(:,1))+sind(a_distanceS1(:,3));
positionS1(:,3) = -1+cosd(a_distanceS1(:,2))+sind(a_distanceS1(:,1));

positionS2(:,1) = -1+cosd(a_distanceS2(:,3))+sind(a_distanceS2(:,2));
positionS2(:,2) = -1+cosd(a_distanceS2(:,1))+sind(a_distanceS2(:,3));
positionS2(:,3) = -1+cosd(a_distanceS2(:,2))+sind(a_distanceS2(:,1));

%plotting
hold on;
set(gcf,'color','white')
subplot(4,1,1)
plot3(positionS1(:,2),positionS1(:,1),positionS1(:,3))
title('3-D Linear Position based on Relative Angular Velocity Sensor 1 (Top)')
xlabel('y Position'),ylabel('x Position'),zlabel('z Position')
xlim([-1 1]), ylim([-2 1]), zlim([-1 1])
grid on

% subplot(3,1,2)
% plot(TestDurationS1,positionS1(:,3))
% title('Z Axis Position Sensor 1 (Top)')
% ylabel('Position (Normalized)'),xlabel('Time (s)')

hold on;
subplot(4,1,2)
plot(TestDurationS2,positionS2(:,1))
title('X Axis Position Sensor 2 (Bottom)')
ylabel('Position (Normalized)'),xlabel('Time (s)')

hold on;
subplot(4,1,3)
plot(TestDurationS2,positionS2(:,2))
title('Y Axis Position Sensor 2 (Bottom)')
ylabel('Position (Normalized)'),xlabel('Time (s)')

hold on;
subplot(4,1,4)
plot(TestDurationS2,positionS2(:,3))
title('Z Axis Position Sensor 2 (Bottom)')
ylabel('Position (Normalized)'),xlabel('Time (s)')