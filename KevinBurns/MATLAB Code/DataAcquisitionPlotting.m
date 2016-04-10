filename = 'm0001_s07_m01_n01.txt';
delimiterIn = '\t';
headerlinesIn = 1;
A = importdata(filename, delimiterIn, headerlinesIn);
GyroXNew = (A.data(:,4))./32.75;
GyroYNew = (A.data(:,5))./32.75;
GyroZNew = (A.data(:,6))./32.75;
x = 1:length(GyroZNew);
y=x./200;
p = polyfit(transpose(y), GyroZNew, 1);
f = polyval(p, transpose(y));
figure(1);
subplot(4, 1, 1), plot(y, GyroXNew), ylabel('Degrees/sec');
subplot(4, 1, 2), plot(y, GyroYNew), ylabel('Degrees/sec');
subplot(4, 1, 3), plot(y, GyroZNew), ylabel('Degrees/sec');
subplot(4, 1, 4), plot(transpose(y), f, '--b');%, hold on,
%plot(GyroYNew), hold on, plot(GyroZNew),ylabel('Degrees/sec');
xlabel('t (seconds)');
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