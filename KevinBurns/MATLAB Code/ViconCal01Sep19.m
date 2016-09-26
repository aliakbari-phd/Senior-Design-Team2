filename = 'Ben_Johnston Cal 01.csv';
V_Base = xlsread(filename, 'C12:E4612');

V_Base_Zeroed_X = V_Base(:,1)-V_Base(1,1);
V_Base_Zeroed_Y = V_Base(:,2)-V_Base(1,2);
V_Base_Zeroed_Z = V_Base(:,3)-V_Base(1,3);

V_Base_Zero(:,1) = V_Base_Zeroed_X;
V_Base_Zero(:,2) = V_Base_Zeroed_Y;
V_Base_Zero(:,3) = V_Base_Zeroed_Z;

Captures = size(V_Base_Zeroed_X);
ViconDuration = Captures(1,1);
ViconDurationSec = ViconDuration/30;

Vlin = linspace(1, ViconDuration, ViconDuration);
VSpace = transpose(Vlin./ViconDurationSec);

Maximum = max(abs(V_Base_Zero));

MaxVector = transpose(Maximum);

True_Max = max(MaxVector);

V_BaseNorm_X = V_Base_Zeroed_X./True_Max;
V_BaseNorm_Y = V_Base_Zeroed_Y./True_Max;
V_BaseNorm_Z = V_Base_Zeroed_Z./True_Max;

% V_Base_Normalized = V_Base_Zeroed_X./V_Base_Zeroed.max;

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

delimiterIn = ' ';
headerlinesIn = 0;

filenameA = 'spinebaseVT1.txt';
A = importdata(filenameA, delimiterIn, headerlinesIn);
SpineBaseX = str2double(A.textdata(:,1));
SpineBaseY = str2double(A.textdata(:,2));
SpineBaseZ = str2double(A.textdata(:,3));
SpineBaseX = SpineBaseX.*100;
SpineBaseY = SpineBaseY.*100;
SpineBaseZ = SpineBaseZ.*100;

SpineBaseX = SpineBaseX - SpineBaseX(1,1);
SpineBaseY = SpineBaseY - SpineBaseY(1,1);
SpineBaseZ = SpineBaseZ - SpineBaseZ(1,1);

SBXMax = max(abs(SpineBaseX));
SBYMax = max(abs(SpineBaseY));
SBZMax = max(abs(SpineBaseZ));

SpineBaseX = SpineBaseX./SBXMax;
SpineBaseY = SpineBaseY./SBYMax;
SpineBaseZ = SpineBaseZ./SBZMax;

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

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

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

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

[Vic_pks, Vic_locs] = findpeaks(V_BaseNorm_X, 'MinPeakProminence',.25);
[IMU_pks, IMU_locs] = findpeaks(positionS2(:,1), 'MinPeakProminence',.25);
[Kin_pks, Kin_locs] = findpeaks(SpineBaseZ, 'MinPeakProminence',.25);



%%%%%%%%%%%PLOTTING HERE%%%%%%%%%%%%%%%%%%

figure(1)
set(gcf, 'color', 'white');

subplot(3,1,1)
%plot(VSpace,V_BaseNorm_X,TestDurationS2, positionS2(:,1),TestDuration,SpineBaseZ)
plot(VSpace,V_BaseNorm_X,VSpace(Vic_locs),Vic_pks,'or')
title('Vicon')

subplot(3,1,2)
plot(TestDurationS2, positionS2(:,1), TestDurationS2(IMU_locs),IMU_pks,'or')
title('IMU')
ylabel('Normalized Position')
ylim([-1 1])

subplot(3,1,3)
plot(TestDuration, SpineBaseZ, TestDuration(Kin_locs), Kin_pks, 'or')
title('Kinect')
xlabel('Time (s)')

% subplot(3,1,2)
% plot(VSpace,V_BaseNorm_Y,TestDurationS2, positionS2(:,2),TestDuration,SpineBaseX)
% title(' Y Position')
% ylabel('Normalized Position')
% ylim([-1 1])

% subplot(3,1,3)
% plot(VSpace,V_BaseNorm_Z,TestDurationS2, positionS2(:,3),TestDuration,SpineBaseY)
% title('Z Position')
% xlabel('Time (s)')





% subplot(4, 1, 1), plot(TestDuration, SpineBaseX), ylabel('PositionX (cm)');
% title('SpineBase');
% subplot(4, 1, 2), plot(TestDuration, SpineBaseY), ylabel('PositionY (cm)');
% subplot(4, 1, 3), plot(TestDuration, SpineBaseZ), ylabel('PositionZ (cm)');
 % subplot(4, 1, 4), plot(TestDuration, SpineBaseTracked), ylabel('IsTracked');
% xlabel('seconds');
