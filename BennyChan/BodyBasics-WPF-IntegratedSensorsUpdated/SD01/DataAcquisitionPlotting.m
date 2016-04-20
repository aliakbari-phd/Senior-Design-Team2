delimiterIn = ' ';
headerlinesIn = 0;

filenameA = 'spinebase.txt';
A = importdata(filenameA, delimiterIn, headerlinesIn);
SpineBaseX = str2double(A.textdata(:,1));
SpineBaseY = str2double(A.textdata(:,2));
SpineBaseZ = str2double(A.textdata(:,3));
VelSpineBaseX = diff(SpineBaseX);
VelSpineBaseX = [0; VelSpineBaseX];
VelSpineBaseY = diff(SpineBaseY);
VelSpineBaseY = [0; VelSpineBaseY];
VelSpineBaseZ = diff(SpineBaseZ);
VelSpineBaseZ = [0; VelSpineBaseZ];
SpineBaseX = SpineBaseX.*100;
SpineBaseY = SpineBaseY.*100;
SpineBaseZ = SpineBaseZ.*100;
VelSpineBaseX = VelSpineBaseX.*100;
VelSpineBaseY = VelSpineBaseY.*100;
VelSpineBaseZ = VelSpineBaseZ.*100;
t = A.data(:,1);
TestDuration = t-t(1);
TestDuration = TestDuration./1000;
x = 1:length(SpineBaseX);
TrackedBase = A.textdata(:,4);
sz = size(SpineBaseX);
SpineBaseTracked = zeros(sz);
for n=x
    if strcmp(TrackedBase(n),'Tracked');
        SpineBaseTracked(n) = 2;
    elseif strcmp(TrackedBase(n),'Inferred');
        SpineBaseTracked(n) = 1;
    else 
        SpineBaseTracked(n) = 0;
    end
end
%TestDurationMovement = TestDuration(1: 151);
figure(1)
set(gcf, 'color', 'white');
subplot(4, 1, 1), plot(TestDuration(1: 151), VelSpineBaseX(1: 151)), ylabel('PositionX (cm)');
title('SpineBase');
subplot(4, 1, 2), plot(TestDuration(1: 151), VelSpineBaseY(1: 151)), ylabel('PositionY (cm)');
subplot(4, 1, 3), plot(TestDuration(1: 151), VelSpineBaseZ(1: 151)), ylabel('PositionZ (cm)');
subplot(4, 1, 4), plot(TestDuration(1: 151), SpineBaseTracked(1: 151)), ylabel('IsTracked');
xlabel('seconds');

filenameB = 'spinemid.txt';
B = importdata(filenameB, delimiterIn, headerlinesIn);
SpineMidX = str2double(B.textdata(:,1));
SpineMidY = str2double(B.textdata(:,2));
SpineMidZ = str2double(B.textdata(:,3));
SpineMidX = SpineMidX.*100;
SpineMidY = SpineMidY.*100;
SpineMidZ = SpineMidZ.*100;
y = 1:length(SpineMidX);
TrackedMid = B.textdata(:,4);
SpineMidTracked = zeros(sz);
for n=y
    if strcmp(TrackedMid(n),'Tracked');
        SpineMidTracked(n) = 2;
    elseif strcmp(TrackedMid(n),'Inferred');
        SpineMidTracked(n) = 1;
    else 
        SpineMidTracked(n) = 0;
    end
end
figure(2)
set(gcf, 'color', 'white');
subplot(4, 1, 1), plot(TestDuration, SpineMidX), ylabel('PositionX (cm)');
title('SpineMid');
subplot(4, 1, 2), plot(TestDuration, SpineMidY), ylabel('PositionY (cm)');
subplot(4, 1, 3), plot(TestDuration, SpineMidZ), ylabel('PositionZ (cm)');
subplot(4, 1, 4), plot(TestDuration, SpineMidTracked), ylabel('IsTracked');
xlabel('seconds');

filenameC = 'spineshoulder.txt';
C = importdata(filenameC, delimiterIn, headerlinesIn);
SpineShoulderX = str2double(C.textdata(:,1));
SpineShoulderY = str2double(C.textdata(:,2));
SpineShoulderZ = str2double(C.textdata(:,3));
SpineShoulderX = SpineShoulderX.*100;
SpineShoulderY = SpineShoulderY.*100;
SpineShoulderZ = SpineShoulderZ.*100;
z = 1:length(SpineShoulderX);
TrackedShoulder = C.textdata(:,4);
SpineShoulderTracked = zeros(sz);
for n=z
    if strcmp(TrackedShoulder(n),'Tracked');
        SpineShoulderTracked(n) = 2;
    elseif strcmp(TrackedShoulder(n),'Inferred');
        SpineShoulderTracked(n) = 1;
    else 
        SpineShoulderTracked(n) = 0;
    end
end
figure(3)
set(gcf, 'color', 'white');
subplot(4, 1, 1), plot(TestDuration, SpineShoulderX), ylabel('PositionX (cm)');
title('SpineShoulder');
subplot(4, 1, 2), plot(TestDuration, SpineShoulderY), ylabel('PositionY (cm)');
subplot(4, 1, 3), plot(TestDuration, SpineShoulderZ), ylabel('PositionZ (cm)');
subplot(4, 1, 4), plot(TestDuration, SpineShoulderTracked), ylabel('IsTracked');
xlabel('seconds');

filenameD = 'rightshoulder.txt';
D = importdata(filenameD, delimiterIn, headerlinesIn);
RightShoulderX = str2double(D.textdata(:,1));
RightShoulderY = str2double(D.textdata(:,2));
RightShoulderZ = str2double(D.textdata(:,3));
RightShoulderX = RightShoulderX.*100;
RightShoulderY = RightShoulderY.*100;
RightShoulderZ = RightShoulderZ.*100;
w = 1:length(RightShoulderX);
TrackedRight = D.textdata(:,4);
RightShoulderTracked = zeros(sz);
for n=w
    if strcmp(TrackedRight(n),'Tracked');
        RightShoulderTracked(n) = 2;
    elseif strcmp(TrackedRight(n),'Inferred');
        RightShoulderTracked(n) = 1;
    else 
        RightShoulderTracked(n) = 0;
    end
end
figure(4)
set(gcf, 'color', 'white');
subplot(4, 1, 1), plot(TestDuration, RightShoulderX), ylabel('PositionX (cm)');
title('RightShoulder');
subplot(4, 1, 2), plot(TestDuration, RightShoulderY), ylabel('PositionY (cm)');
subplot(4, 1, 3), plot(TestDuration, RightShoulderZ), ylabel('PositionZ (cm)');
subplot(4, 1, 4), plot(TestDuration, RightShoulderTracked), ylabel('IsTracked');
xlabel('seconds');

figure(5)
set(gcf, 'color', 'white');
plot3(SpineShoulderX, SpineShoulderZ, SpineShoulderY);
xlabel('PositionX (cm)');
ylabel('PositionZ (cm)');
zlabel('PositionY (cm)');
hold on;
%plot(RightShoulderZ, RightShoulderY);
%hold on;
plot3(SpineMidX, SpineMidZ, SpineMidY);
hold on;
plot3(SpineBaseX, SpineBaseZ, SpineBaseY);
title('Session 1 Test');