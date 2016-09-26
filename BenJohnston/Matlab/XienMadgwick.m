%% Library
addpath('ximu_matlab_library');
addpath('MahonyAHRS');
addpath('quaternion_library');
addpath('Conversion_folder');


clc;
clear;
%% Importing Data to program

filename = 'C:\Users\xien\Documents\Texas A&M\Terraswarm Team Texas A&M\Nasa Code\MainWindow Kinect Application\MainWindow Kinect Application\Textfiles\ThirdTest_IMU.txt' ; % 7.txt
[A,delimiterOut1] = importdata(filename);

filename = 'C:\Users\xien\Documents\Texas A&M\Terraswarm Team Texas A&M\Nasa Code\MainWindow Kinect Application\MainWindow Kinect Application\Textfiles\ThirdTest_kinect.txt' ; % 7.txt
[B,delimiterOut] = importdata(filename);

samplePeriod = 1/256;
% import accelerometer Data
acc(:,1) = (A(:,1))./4096;
acc(:,2) = (A(:,2))./4096;
acc(:,3) = (A(:,3))./4096;
acc(:,1) = acc(:,1);
acc(:,2) = acc(:,2);



%import data into new gyro table
gyro(:,1) = (A(:,4))./32.75; %datasheet or dataset firmware parameters 9150
gyro(:,2) = (A(:,5))./32.75; %set the sensitivity
gyro(:,3) = (A(:,6))./32.75;  %correction data
gyro(:,1) = gyro(:,1);
gyro(:,2) = gyro(:,2);


x_filter = designfilt( 'lowpassiir' , 'FilterOrder' ,3, ...
     'PassbandFrequency' ,10e3, 'PassbandRipple' ,0.5, ...
     'SampleRate' ,200e3);
 
mag(:,1) = A(:,7);
mag(:,2) = A(:,8);
mag(:,3) = A(:,9);

% import Kinect data
K(:,1) = B(:,1);


%% Calculate time for the imu and the Kinect

Ltime(1,:) = (A(:,10))./1000;
T = (Ltime(end)-Ltime(1))/length(Ltime); %find average period
time = (1:length(Ltime)).*T; %create time vector based on average period
% gyro=cumtrapz(time,gyro);

% Kinect time
Ktime(1,:) = (B(:,2))./1000;
KT = (Ktime(end)-Ktime(1))/length(Ktime);
ktime = (1:length(Ktime)).*KT;
accZero = zeros(length(time),3);
for f=1:length(time)
   accZero(f,:) = acc(1,:);
end

%% Process the sensor into the Orientation algorithm

AHRS = MadgwickAHRS('SamplePeriod', 1/200, 'Beta', 0.5);
% AHRS = MahonyAHRS('SamplePeriod', 1/200, 'Kp', 0.5);

quaternion = zeros(length(time), 4);
for t = 1:length(time)
    AHRS.Update(gyro(t,:) * (pi/180), acc(t,:),mag(t,:));	% gyroscope units must be radians
    quaternion(t, :) = AHRS.Quaternion;
end

%% find algorithm output as Euler angles
% The first and third Euler angles in the sequence (phi and psi) become
% unreliable when the middle angles of the sequence (theta) approaches ±90
% degrees. This problem commonly referred to as Gimbal Lock.
% use quaternion2euler method

euler = quatern2euler(quaternConj(quaternion))*(180/pi) ;	% use conjugate for sensor frame relative to Earth and convert to degrees.
% OldEuler = euler(1,:);

% convert = 123; %the output of the Euler angles. 123 = xyz
% 
% initRotMat = euler2rotMat(euler(1,1),euler(1,2),euler(1,3));
% rotMatConj = transpose(initRotMat);
% rotMat = euler2rotMat(euler(:,1),euler(:,2),euler(:,3));
% 
% for h=1:length(rotMat)
% newRotMat(:,:,h) = rotMat(:,:,h).*rotMatConj;
% newEuler(h,:) = rotMat2euler(newRotMat(:,:,h))* (180/pi);
% end

% normalization(:,1) = sqrt((euler(:,1).*euler(:,1))+(euler(:,2).*euler(:,2))+(euler(:,3).*euler(:,3)));
% normal=normalization;
% normal = euler(:,3);




% for i=1:length(normal)
%     if (i==length(normal)-100)
%         break;
%     end
%     window = abs(normal(i,:)-normal(i:i+100,:));
%     offset = mean(window);
%     
%     
%     if (offset < 1)
%         normal = normal(:,1) - normal(i,:);
%         
%         break;
%     end
% end
% 
% alpha = zeros(length(K),1);
% tolerance = 0.40;
% inc = 1;
% %export combine data
% 
% data = zeros(length(ktime),5);
% 
% for num = 1:length(ktime)
%     a= .1;
%     for num2 = 1:length(time)
%         
%           b = abs(ktime(1,num) - time(1,num2));%the difference between two values. finding the nearest neighbor of two values.
%           if b <a
%               a=b;
%               temp = time(1,num2);%grab time from imu
%               temp2 = normal(num2,1); %grab imu data
%           end    
%     end
%     data(num,4) = K(num,1); %grab Kinect data export it to "data"
%     data(num,3) = temp2;    %grab IMU data and export it to "data"
%     data(num,2) = temp;
%     data(num,1) = ktime(1,num);
% end
% for num3 = 1:length(data)
%     
%     if(num3 == length(data))
%          break;
%     end
%     %grab alpha
%     data(num3,5) = abs(K(num3,1)- K((num3+1),1));   %this is the difference between two values of kinect    
% end
% 
% fuseData = zeros(length(data),1);
% % value=0;
% % mean =1;
% for num4=1:length(data)
% %     value = value + data(num4,3);
%      if(data(num4,5) <= tolerance)
%         %IMU reset %subtract Offset value everytime
%         %offset is already subtracted
%         
%         
%         %capture kinect
%         fuseData(num4,1) = data(num4,4);
%      else
%         %capture IMU
%         fuseData(num4,1) = data(num4,3);
%     end
% end
%      
% %         if alpha <= tolerance
% %             %reset IMU
% %             %capture Kinect
% %              fuseData(num,1)=K(num,1);
% %         end
% %         fuseData(num,1) = normal(inc,1);
% %         inc = inc + 1;
% %     end
% %     
% %   error  
% xi = data(:,4);
% x0=data(:,3);
% var = xi - x0;
% nvar = var.*var;
% sum = 0;
% for v=1:length(data);
%     sum = sum + nvar(v,1); %RMS
% end
% nevar = sum/length(data);
% 
% rms = sqrt(nevar);

% acc = cumtrapz(time,acc);
% gyro = cumtrapz(time,gyro);
% %convert it back to degrees (value).*180/pi
% figure('Name', 'Euler Angles');
hold on;
% plot(time, normal, 'k');
plot(ktime, K, 'g');
% plot(ktime, rms, 'r');
% plot(time, euler(:,1), 'r');
plot(time, euler(:,2), 'm');
% plot(time, euler(:,3), 'r');
%plot( ktime, fuseData, 'b');
%    plot( ktime, data(:,4), 'r');
%  plot( ktime, data(:,3), 'r');
% plot(time,acc(:,1),'r');
% plot(time,acc(:,2),'b');
% plot(time,acc(:,3),'m');
% plot(time,gyro(:,1),'r');
% plot(time,gyro(:,2),'b');
% plot(time,gyro(:,3),'m');
% plot(time,newEuler(:,1),'k');
% plot(time,newEuler(:,2),'b');
% plot(time,newEuler(:,3),'r');
title('Displacement angles');
xlabel('Time (s)');
ylabel('Angle (\circ)');
legend('Kinect','fusion','IMU');
hold off;