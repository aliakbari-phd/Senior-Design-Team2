clear;
clc;
%%%%%%%%%%%%%%%%%%%%%%%%%%%%  VICON  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
filename = 'Ben_Johnston Cal 02.csv';
V_Data = xlsread(filename, 'A12:N1619');

pnts_base(:,1) = V_Data(:,3);           %base points
pnts_base(:,2) = V_Data(:,4);
pnts_base(:,3) = V_Data(:,5);

pnts_upper(:,1) = V_Data(:,9);          %upper points
pnts_upper(:,2) = V_Data(:,10);
pnts_upper(:,3) = V_Data(:,11);

v_zunit = ([0 0 1]);            %create z unit vector
Vic_frames = V_Data(:,1);


v_pntpnt = pnts_upper - pnts_base;      %point to point vector

iterator_a=1;
v_length = size(v_pntpnt);
l = v_length(1,1);
while iterator_a<l
v_pnt_norm = v_pntpnt(iterator_a,:)./norm(v_pntpnt(iterator_a,:));
iterator_a = iterator_a+1;
theta(iterator_a,:) = acos(dot(v_pnt_norm,v_zunit));
end
alpha = (pi/2)-theta;
alpha_deg = alpha.*(180/pi);

%Syncing
[Vic_pks, Vic_locs] = findpeaks(alpha_deg, 'MinPeakProminence', .5);

Vic_peak_beg = Vic_locs(1);
Vic_peak_end = Vic_locs(end);

Frames_used = Vic_frames(Vic_peak_end)-Vic_frames(Vic_peak_beg);

Vic_time = (Frames_used)/100;
Vic_plot_yaxis = alpha_deg(Vic_peak_beg:Vic_peak_end);
Vic_plot_xaxis = 0:Vic_time/(Frames_used):Vic_time;


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%  IMUs  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%NOTE:
%Need to import GyroZ and Ltime columns from Bapgui

filenameSMid = 'T2S1.txt';
filenameSBase = 'T2S2.txt';
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

[SMid_pks , SMid_locs] = findpeaks(distanceMid(:,2), 'MinPeakProminence', 2);

% plot(tMid, distanceMid(:,2), tMid(SMid_locs), SMid_pks, 'or');

IMU_str2end_frame = SMid_locs(end)-SMid_locs(1);
IMU_str2end_time = tMid(SMid_locs(end))-tMid(SMid_locs(1));
IMU_timesteps = IMU_str2end_time/IMU_str2end_frame;

IMU_prev_time = IMU_str2end_time;
IMU_prev_frms = 2/IMU_timesteps;

SMid_y_axis = distanceMid(:,2);

SMid_peak_beg = SMid_locs(1);
SMid_peak_end = SMid_locs(end);

% 
% Frames_used = Vic_frames(Vic_peak_end)-Vic_frames(Vic_peak_beg);
% 
SMid_time = IMU_prev_time;
SMid_plot_yaxis = SMid_y_axis(SMid_peak_beg:SMid_peak_end);
SMid_plot_xaxis = 0:SMid_time/(SMid_peak_end-SMid_peak_beg):SMid_time;


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%  KINECT  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%





%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% ENTER NAME  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%



%Vic_plot_yaxis

%%%%%%%%%%%%%%%%%%%%%%%%%%%  PLOTTING  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% subplot(3,1,1)
plot(Vic_plot_xaxis,Vic_plot_yaxis-mean(Vic_plot_yaxis),SMid_plot_xaxis, SMid_plot_yaxis-mean(SMid_plot_yaxis))
xlim([0 Vic_time])
title('Angular Distance (deg)')
ylabel('x'),xlabel('Time (s)')

% subplot(3,1,2)

% xlim([0 SMid_time])
% ylabel('y'),xlabel('Time (s)')


%subplot(3,1,3)
%plot(tMid,distanceMid(:,3), tBase,distanceBase(:,3))
%ylabel('z'),xlabel('Time (s)')


SMid_plot_yaxis = resample(SMid_plot_yaxis,length(Vic_plot_yaxis),length(SMid_plot_yaxis));
SMid_plot_xaxis = resample(SMid_plot_xaxis,length(Vic_plot_xaxis),length(SMid_plot_xaxis));
% Kin_plot_y = resample(Kin_plot_y,length(Vic_plot_yaxis),length(Kin_plot_y));
% Kin_plot_time = resample(Kin_plot_time,length(Vic_plot_xaxis),length(Kin_plot_time));

angleRMSE_IMU = sqrt(mean((Vic_plot_yaxis - SMid_plot_yaxis).^2))
% angleRMSE_Kin = sqrt(mean((Vic_plot_yaxis - Kin_plot_y).^2))