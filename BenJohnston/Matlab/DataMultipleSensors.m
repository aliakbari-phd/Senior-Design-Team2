%NOTE:
%Need to import GyroZ and Ltime columns from Bapgui

filenameSMid = 'T1S1.txt';
filenameSBase = 'T1S2.txt';
delimiterIn = ' ';
headerlinesIn = 1;
SMid = importdata(filenameSMid, delimiterIn, headerlinesIn);
SBase = importdata(filenameSBase, delimiterIn, headerlinesIn);

%Program reports data using the z-axis of the gyroscope
%including angular position, velocity, acceleration and jerk,
%includes correction factor

%import data
gyroMid(:,1) = (SMid.data(:,4))./32.75;  
gyroMid(:,2) = (SMid.data(:,5))./32.75;  
gyroMid(:,3) = (SMid.data(:,6))./32.75;          %Gyroscope correction factor
gyroBase(:,1) = (SBase.data(:,4))./32.75;  
gyroBase(:,2) = (SBase.data(:,5))./32.75;  
gyroBase(:,3) = (SBase.data(:,6))./32.75;          %Gyroscope correction factor
LtimeMid = (SMid.data(:,10));
LtimeBase = (SBase.data(:,10));
tMid = transpose((LtimeMid-LtimeMid(1))./1000);     %relative to start time, ms to s
tBase = transpose((LtimeBase-LtimeBase(1))./1000);



%*******low pass filter*****
x_filter = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',10e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);
gyroMid = filtfilt(x_filter,gyroMid);
gyroBase = filtfilt(x_filter,gyroBase);


%best fit code
%t3(:,1) = transpose(t);
%t3(:,2) = transpose(t);
%t3(:,3) = transpose(t);
%p1 = polyfit(t3,gyro,1);
%c_velocity = transpose(polyval(p1,t));      %best fit line of velocity data


%kinect corrections
%IMU_coeff = polyfit(transpose(IMU_plot_x),IMU_plot_func,1);
%IMU_bestfit = transpose(polyval(IMU_coeff,IMU_plot_x));

%mean_kinect = mean(Kin_plot_func);
%mean_kinect_line = ones([length(Kin_plot_func),1]);
%mean_kinect_line = mean_kinect_line .* mean_kinect;

%IMU_fusion = IMU_bestfit.*(-1);
%IMU_fusion = IMU_fusion + mean_kinect;
%IMU_corrected_func = IMU_plot_func + IMU_fusion;
%mean_IMU_line = ones([length(IMU_plot_func),1]).*mean_kinect;



%differentiation and integration
accelerationMid = diff(gyroMid);             % vel to accel 
accelerationMid = [0,[1 3];accelerationMid];
jerkMid = diff(accelerationMid);             %accel to jerk
jerkMid = [0,[1 3];jerkMid];
positionMid = trapz(tMid,gyroMid);
distanceMid = cumtrapz(tMid,gyroMid);     % vel to distance
distanceMid(:,2) = distanceMid(:,2) + 90;

accelerationBase = diff(gyroBase);             % vel to accel 
accelerationBase = [0,[1 3];accelerationBase];
jerkBase = diff(accelerationBase);             %accel to jerk
jerkBase = [0,[1 3];jerkBase];
positionBase = trapz(tBase,gyroBase);
distanceBase = cumtrapz(tBase,gyroBase);     % vel to distance
distanceBase(:,2) = distanceBase(:,2) + 90;



%subplot(3,1,1)
%plot(tMid,distanceMid(:,1),tBase,distanceBase(:,1))
title('Angular Distance (deg)')
%ylabel('x'),xlabel('Time (s)')

%subplot(3,1,2)
plot(tMid,distanceMid(:,2),tBase,distanceBase(:,2))
ylabel('y'),xlabel('Time (s)')
legend('Middle Sensor', 'Base Sensor')

%subplot(3,1,3)
%plot(tMid,distanceMid(:,3), tBase,distanceBase(:,3))
%ylabel('z'),xlabel('Time (s)')



%results
%abs_distance = cumtrapz(t,abs(gyro));

%result_duration = t(end);                   %report duration of test
%result_Fs = length(gyro)/t(end);       %Fs = sampling frequency
%result_xdistance = abs(abs_distance(end,1));      %report final distance
%result_ydistance = abs(abs_distance(end,2));
%result_zdistance = abs(abs_distance(end,3));  
%result_drift = abs(gyro(end,1)-gyro(onepercent,1));
%result_bfdrift = abs(c_velocity(end)-c_velocity(1)); %difference in begin to end of best fit velocity
%result_mean_vel = abs(mean(gyro(:,3)));   %report mean velocity
