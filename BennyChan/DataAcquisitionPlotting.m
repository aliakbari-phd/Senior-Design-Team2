delimiterIn = ' ';
headerlinesIn = 0;

filenameA = 'spinebase.txt';
A = importdata(filenameA, delimiterIn, headerlinesIn);
SpineBaseX = str2double(A.textdata(:,1));
SpineBaseY = str2double(A.textdata(:,2));
SpineBaseZ = str2double(A.textdata(:,3));
x = 1:length(SpineBaseX);
figure(1)
subplot(3, 1, 1), plot(SpineBaseX), ylabel('PositionX');
title('SpineBase');
subplot(3, 1, 2), plot(SpineBaseY), ylabel('PositionY');
subplot(3, 1, 3), plot(SpineBaseZ), ylabel('PositionZ');
xlabel('Sample');

filenameB = 'spinemid.txt';
B = importdata(filenameB, delimiterIn, headerlinesIn);
SpineMidX = str2double(B.textdata(:,1));
SpineMidY = str2double(B.textdata(:,2));
SpineMidZ = str2double(B.textdata(:,3));
y = 1:length(SpineMidX);
figure(2)
subplot(3, 1, 1), plot(SpineMidX), ylabel('PositionX');
title('SpineMid');
subplot(3, 1, 2), plot(SpineMidY), ylabel('PositionY');
subplot(3, 1, 3), plot(SpineMidZ), ylabel('PositionZ');
xlabel('Sample');

filenameC = 'spineshoulder.txt';
C = importdata(filenameC, delimiterIn, headerlinesIn);
SpineShoulderX = str2double(C.textdata(:,1));
SpineShoulderY = str2double(C.textdata(:,2));
SpineShoulderZ = str2double(C.textdata(:,3));
z = 1:length(SpineShoulderX);
figure(3)
subplot(3, 1, 1), plot(SpineShoulderX), ylabel('PositionX');
title('SpineShoulder');
subplot(3, 1, 2), plot(SpineShoulderY), ylabel('PositionY');
subplot(3, 1, 3), plot(SpineShoulderZ), ylabel('PositionZ');
xlabel('Sample');

figure(4)
plot(SpineShoulderX, SpineShoulderY);
title('Test');
xlabel('Sample');