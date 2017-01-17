clc;
clear;
%%%%%%%%%%%%%%%%%%%%%%%%%%%%  VICON  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
filename = 'Ben_Johnston Cal 02.csv';
V_Data = xlsread(filename, 'A11:N1618');
filename1_Kin = 'spinebaseT2.txt';
filename2_Kin = 'spinemidT2.txt';
filenameSMid = 'T2S1.txt';
filenameSBase = 'T2S2.txt';

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
Vic_t = Vic_frames./100;

%%%%% Position Filter
UnivFilt = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',5e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);

VPos_Filtered = filtfilt(UnivFilt, Vic_plot_yaxis);       
 
%%%%% First Derivative for flex (Angular Velocity)
VPosderiv = diff(Vic_plot_yaxis);
VTimderiv = diff(Vic_plot_xaxis);
VTimderiv = transpose(VTimderiv);
VVel_1 = VPosderiv./VTimderiv;
VVel = [0;VVel_1];

%%%%% Velocity Filter
VVelFilt = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',5e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);
VVel_Filtered = filtfilt(VVelFilt, VVel);

%%%%% Second Derivative for flex (Angular Acceleration)
VVelderiv = diff(VVel);
VAcc_1 = VVelderiv./VTimderiv;
VAcc = [0;VAcc_1];

%%%%% Acceleration Filter
VAccFilt = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',5e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);
VAcc_Filtered = filtfilt(VAccFilt,VAcc);

%%%%% Third Derivative for flex (Angular Jerk)
VAccderiv = diff(VAcc);
VJer_1 = VAccderiv./VTimderiv;
VJer = [0;VJer_1];

%%%%% Jerk Filter
VJerFilt = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',5e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);
VJer_Filtered = filtfilt(VJerFilt,VJer);

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%  IMUs  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

%NOTE:
%Need to import GyroZ and Ltime columns from Bapgui


delimiterIn = ' ';
headerlinesIn_IMU = 1;
SMid = importdata(filenameSMid, delimiterIn, headerlinesIn_IMU);
SBase = importdata(filenameSBase, delimiterIn, headerlinesIn_IMU);

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
freqMid = length(tMid)/(tMid(end)-tMid(1));
tMid = 0:1/freqMid:tMid(end-1);
tMid = resample(tMid, length(gyroMid), length(tMid));

%*******low pass filter*****
x_filter = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',5e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);
gyroMid = filtfilt(x_filter,gyroMid);
gyroBase = filtfilt(x_filter,gyroBase);

positionMid = trapz(tMid,gyroMid);
distanceMid = cumtrapz(tMid,gyroMid);     % vel to distance
distanceMid(:,2) = distanceMid(:,2) + 90;

accMid = diff(gyroMid)./(1/freqMid);             % vel to accel 
accMid = [0,[1 3];accMid];
jMid = diff(accMid)./(1/freqMid);             %accel to jerk
jMid = [0,[1 3];jMid];
positionBase = trapz(tBase,gyroBase);
distanceBase = cumtrapz(tBase,gyroBase);     % vel to distance
distanceBase(:,2) = distanceBase(:,2) + 90;

[SMid_pks , SMid_locs] = findpeaks(distanceMid(:,2), 'MinPeakProminence', 2);

IMU_str2end_frame = SMid_locs(end)-SMid_locs(1);
IMU_str2end_time = tMid(SMid_locs(end))-tMid(SMid_locs(1));
IMU_timesteps = IMU_str2end_time/IMU_str2end_frame;

IMU_prev_time = IMU_str2end_time;
IMU_prev_frms = 2/IMU_timesteps;

SMid_y_axis = distanceMid(:,2);

SMid_peak_beg = SMid_locs(1);
SMid_peak_end = SMid_locs(end);

SMid_time = IMU_prev_time;
SMid_plot_yaxis = SMid_y_axis(SMid_peak_beg:SMid_peak_end);
SMid_plot_xaxis = 0:SMid_time/(SMid_peak_end-SMid_peak_beg):SMid_time;

%%%%% Kinect %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
delimiterIn = ' ';
headerlinesIn_Kin = 0;
spinebaseData = importdata(filename1_Kin, delimiterIn, headerlinesIn_Kin);
spinemidData = importdata(filename2_Kin, delimiterIn, headerlinesIn_Kin);

time = spinebaseData.data(:,1);
time = time - time(1);
time = transpose(time./1000);

pnts_base_Kin(:,1) = str2double(spinebaseData.textdata(:,1));          %base points
pnts_base_Kin(:,2) = str2double(spinebaseData.textdata(:,2));
pnts_base_Kin(:,3) = str2double(spinebaseData.textdata(:,3));

pnts_upper_Kin(:,1) = str2double(spinemidData.textdata(:,1));         %upper points
pnts_upper_Kin(:,2) = str2double(spinemidData.textdata(:,2));
pnts_upper_Kin(:,3) = str2double(spinemidData.textdata(:,3));

kinect_yunit = ([0 1 0]);            %create z unit vector

kinect_pntpnt = pnts_upper_Kin - pnts_base_Kin;      %point to point vector

iterator_a=1;
kinect_length = size(kinect_pntpnt);
l = kinect_length(1,1);
while iterator_a<l
kinect_pnt_norm = kinect_pntpnt(iterator_a,:)./norm(kinect_pntpnt(iterator_a,:));
iterator_a = iterator_a+1;
theta_Kin(iterator_a,:) = acos(dot(kinect_pnt_norm,kinect_yunit));
end
alpha_Kin = (pi/2)-theta_Kin;
alpha_deg_Kin = alpha_Kin.*(180/pi);

kin_filter = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',30e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);

alpha_deg_Kin_filt = filtfilt(kin_filter, alpha_deg_Kin);

[Kin_pks , Kin_locs] = findpeaks(alpha_deg_Kin_filt, 'MinPeakProminence', .2);

Kin_Frames=Kin_locs(end)-Kin_locs(1);
Kin_time_diff = time(Kin_locs(end))-time(Kin_locs(1));
Kin_time_step = Kin_time_diff/Kin_Frames;
Kin_added_time = round(2/.0333);



Kin_pks_begin = Kin_locs(1);
Kin_pks_end = Kin_locs(end);

Kin_plot_time = 0:Kin_time_diff/(Kin_locs(end)-Kin_locs(1)):Kin_time_diff;
Kin_plot_y = alpha_deg_Kin_filt(Kin_pks_begin:Kin_pks_end);

%%%%% Correction of IMUs (DRIFT) %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
[IMU_peaks, IMU_locs] = findpeaks(SMid_plot_yaxis, 'MinPeakProminence', 2);

IMU_locs = [1; IMU_locs];
IMU_locs = [IMU_locs; length(SMid_plot_xaxis)];
IMU_peaks = [SMid_plot_yaxis(1); IMU_peaks];
IMU_peaks = [IMU_peaks; SMid_plot_yaxis(end)];
IMU_times = IMU_locs./freqMid;

IMU_coeff = zeros([length(IMU_times)-1 2]);

for i = 1:length(IMU_peaks)-1
    IMU_coeff(i,1) = (IMU_peaks(i+1) - IMU_peaks(i))/(IMU_times(i+1)-IMU_times(i));
    IMU_coeff(i,2) = IMU_peaks(i)-(IMU_coeff(i,1)*IMU_times(i));
end
IMU_bestfit = zeros(length(SMid_plot_xaxis),1);

for i = 1:length(IMU_peaks)-1
    IMU_bestfit(IMU_locs(i):IMU_locs(i+1)) = transpose(polyval(IMU_coeff(i,:),SMid_plot_xaxis(IMU_locs(i):IMU_locs(i+1))));
end


freqKin = length(time)/(time(end)-time(1));
[Kin_peaks, Kin_locations] = findpeaks(Kin_plot_y, 'MinPeakProminence', 62);
Kin_locations = [1; Kin_locations];
Kin_locations = [Kin_locations; length(Kin_plot_time)];
Kin_peaks = [Kin_plot_y(1); Kin_peaks];
Kin_peaks = [Kin_peaks; Kin_plot_y(end)];
Kin_times = Kin_locations./freqKin;
Kin_coeff = zeros([length(Kin_times)-1 2]);

for i = 1:length(Kin_peaks)-1
    Kin_coeff(i,1) = (Kin_peaks(i+1) - Kin_peaks(i))/(Kin_times(i+1)-Kin_times(i));
    Kin_coeff(i,2) = Kin_peaks(i)-(Kin_coeff(i,1)*Kin_times(i));
end
Kin_bestfit = zeros(length(SMid_plot_xaxis),1);

for i = 1:length(Kin_peaks)-1
    Kin_bestfit(IMU_locs(i):IMU_locs(i+1)) = transpose(polyval(Kin_coeff(i,:),SMid_plot_xaxis(IMU_locs(i):IMU_locs(i+1))));
end


IMU_fusion = Kin_bestfit - IMU_bestfit;
IMU_corrected_func = SMid_plot_yaxis + IMU_fusion;

velMid = diff(IMU_corrected_func)./(1/freqMid);
velMid = [0;velMid];
accelerationMid = diff(velMid)./(1/freqMid);             % vel to accel 
accelerationMid = [0;accelerationMid];
jerkMid = diff(accelerationMid)./(1/freqMid);             %accel to jerk
jerkMid = [0;jerkMid];

%%%%% Resampling and Sync %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
Vic_frames = Vic_frames./100;
tMid = resample(tMid, length(Vic_plot_xaxis), length(tMid));
IMU_corrected_func = resample(IMU_corrected_func, length(Vic_plot_xaxis),length(IMU_corrected_func));
velMid = resample(velMid, length(Vic_plot_xaxis),length(velMid));
accelerationMid = resample(accelerationMid,length(Vic_plot_xaxis),length(accelerationMid));
jerkMid = resample(jerkMid,length(Vic_plot_xaxis),length(jerkMid));

tMid = filtfilt(UnivFilt, tMid);
IMU_corrected_func = filtfilt(UnivFilt, IMU_corrected_func);
velMid = filtfilt(UnivFilt, velMid);
accelerationMid = filtfilt(UnivFilt, accelerationMid);
jerkMid = filtfilt(UnivFilt, jerkMid);

%%%%% Plotting %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
figure(1)
%subplot(4,1,1)
plot(Kin_plot_time, Kin_plot_y, Kin_plot_time, Kin_bestfit, SMid_plot_xaxis, IMU_bestfit, SMid_plot_xaxis, SMid_plot_yaxis)
title('Vicon Parameters')
ylabel('degrees'),xlabel('Time (s)')

subplot(4,1,2)
plot(Vic_plot_xaxis,VVel_Filtered,Vic_plot_xaxis,velMid)
ylabel('degrees/s'),xlabel('Time (s)')

subplot(4,1,3)
plot(Vic_plot_xaxis,VAcc_Filtered,Vic_plot_xaxis,accelerationMid)
ylim([-2000 2000])
ylabel('degrees/s^2'),xlabel('Time (s)')

subplot (4,1,4)
plot(Vic_plot_xaxis,VJer_Filtered,Vic_plot_xaxis,jerkMid)
ylabel('degrees/s^3'),xlabel('Time (s)')
legend('Vicon','Corrected IMU')

%%%%% RMSE

RMSEangle = sqrt(mean((VPos_Filtered - IMU_corrected_func).^2))
RMSEvelocity = sqrt(mean((VVel_Filtered - velMid).^2))
RMSEacceleration = sqrt(mean((VAcc_Filtered - accelerationMid).^2))
RMSEjerk = sqrt(mean((VJer_Filtered - jerkMid).^2))


%%%%% Maximums
VVel_abs = abs(VVel);
VAcc_abs = abs(VAcc_Filtered);
VJer_abs = abs(VJer_Filtered);
IMUVel_abs = abs(velMid);
IMUAcc_abs = abs(accelerationMid);
IMUJer_abs = abs(jerkMid);

MaxVelVicon = max(VVel_abs)
MaxAccVicon = max(VAcc_abs)
MaxJerVicon = max(VJer_abs)
MaxVelIMU = max(IMUVel_abs)
MaxAccIMU = max(IMUAcc_abs)
MaxJerIMU = max(IMUJer_abs)