filename = '15.txt';
delimiterIn = ' ';
headerlinesIn = 1;
S = importdata(filename, delimiterIn, headerlinesIn);
GyroXNew = (S.data(:,4))./32.75;
GyroYNew = (S.data(:,5))./32.75;
GyroZNew = (S.data(:,6))./32.75;
k = S.data(:,10);
TestDurationS = k-k(1);
TestDurationS = TestDurationS./1000;
%p = polyfit(transpose(k), GyroZNew, 1);
%f = polyval(p, transpose(k));
figure(7);
set(gcf, 'color', 'white');
subplot(3, 1, 1), plot(TestDurationS, GyroXNew), ylabel('X Degrees/sec');
title('MotionNet');
subplot(3, 1, 2), plot(TestDurationS, GyroYNew), ylabel('Y Degrees/sec');
subplot(3, 1, 3), plot(TestDurationS, GyroZNew), ylabel('Z Degrees/sec');
%subplot(4, 1, 4), plot(transpose(x), f, '--b');%, hold on,
%plot(GyroYNew), hold on, plot(GyroZNew),ylabel('Degrees/sec');
xlabel('seconds');
% AcclXNew = AcclX./4096;
% AcclYNew = AcclY./4096;
% AcclZNew = AcclZ./4096;
% figure(2);
% subplot(4, 1, 1), plot(AcclXNew), ylabel('Degrees/sec');
% subplot(4, 1, 2), plot(AcclYNew), ylabel('Degrees/sec');
% subplot(4, 1, 3), plot(AcclZNew), ylabel('Degrees/sec');
% subplot(4, 1, 4), plot(AcclXNew), hold on,
% plot(AcclYNew), hold on, plot(AcclZNew),
% xlabel('Sample'), ylabel('Degrees/sec');

figure(6)
set(gcf, 'color', 'white');
plot3(GyroXNew, GyroZNew, GyroYNew);
title('Test');
xlabel('Sample');