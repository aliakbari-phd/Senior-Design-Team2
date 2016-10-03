filename = 'ViconTest12.txt';
delimiterIn = ' ';
headerlinesIn = 1;
S = importdata(filename, delimiterIn, headerlinesIn);
GyroXNew = (S.data(:,4))./32.75;
GyroYNew = (S.data(:,5))./32.75;
GyroZNew = (S.data(:,6))./32.75;
k = S.data(:,10);
TestDurationS = k-k(1);
TestDurationS = TestDurationS./1000;

hold on;
set(gcf,'color','white')
subplot(4,1,1)
plot3(GyroXNew, GyroYNew, GyroZNew)
title('3-D Linear Position based on Relative Angular Velocity Sensor 1 (Top)')
xlabel('y Position'),ylabel('x Position'),zlabel('z Position')
xlim([-1 1]), ylim([-2 1]), zlim([-1 1])
grid on

hold on;
subplot(4,1,2)
plot(TestDurationS, GyroXNew)
title('X Axis Position Sensor 2 (Bottom)')
ylabel('Position (Normalized)'),xlabel('Time (s)')

hold on;
subplot(4,1,3)
plot(TestDurationS, GyroYNew)
title('Y Axis Position Sensor 2 (Bottom)')
ylabel('Position (Normalized)'),xlabel('Time (s)')

hold on;
subplot(4,1,4)
plot(TestDurationS, GyroZNew)
title('Z Axis Position Sensor 2 (Bottom)')
ylabel('Position (Normalized)'),xlabel('Time (s)')