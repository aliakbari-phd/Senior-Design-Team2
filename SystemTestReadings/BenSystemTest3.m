clc;
clear;

%%%%%%%%%%%%%%%%%%%%%%%%%%%%  VICON  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
filename = 'Ben_Johnston Cal 04.csv';
V_Data = xlsread(filename, 'A12:N1562');

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

filenameSMid = 'T4S1.txt';
filenameSBase = 'T4S2.txt';
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

filename1_Kin = 'spinebaseT4.txt';
filename2_Kin = 'spinemidT4.txt';
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
            'PassbandFrequency',15e3,'PassbandRipple',0.5,...
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
% Kin_plot_time(end+1) = 13.06;
Kin_plot_y = alpha_deg_Kin_filt(Kin_pks_begin:Kin_pks_end);
%plot(Kin_plot_time, Kin_plot_y)



%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% ENTER NAME  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


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
[Kin_peaks, Kin_locations] = findpeaks(Kin_plot_y, 'MinPeakProminence', 2);
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


%%%%%%%%%%%%%%%%%%%%%%%%%%%  PLOTTING  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%subplot(2,1,1)
%plot(Vic_plot_xaxis,Vic_plot_yaxis,SMid_plot_xaxis, IMU_corrected_func, 'green', SMid_plot_xaxis, SMid_plot_yaxis,'red')
%xlim([0 Vic_time])
%title('Angular Distance (deg)')
%ylabel('x'),xlabel('Time (s)')
%legend('Vicon','Corrected', 'Uncorrected')

t = 0:length(Kin_bestfit)-1;
t = t./freqMid;

%plot(t, Kin_bestfit, Kin_plot_time, Kin_plot_y, SMid_plot_xaxis, IMU_bestfit, SMid_plot_xaxis, SMid_plot_yaxis)
%legend('Kinect Best Fit', 'Kinect', 'IMU Best Fit', 'IMU')
figure(1)
subplot(4,1,1)
plot(SMid_plot_xaxis, IMU_corrected_func)
ylabel('Angle (deg)')
subplot(4,1,2)
plot(SMid_plot_xaxis, velMid)
ylabel('Velocity (deg/s)')
subplot(4,1,3)
plot(SMid_plot_xaxis, accelerationMid)
ylabel('Acceleration (deg/s^2)')
ylim([-2000 2000])
subplot(4,1,4)
plot(SMid_plot_xaxis, jerkMid)
ylim([-50000 50000])
ylabel('(deg/s^3)'), xlabel('Time (s)')

figure(2)
subplot(4,1,1)
plot(tMid, distanceMid(:,1))
ylabel('Angle (deg)')
subplot(4,1,2)
plot(tMid, gyroMid(:,1))
ylabel('Velocity (deg/s)')
subplot(4,1,3)
plot(tMid, accMid(:,1))
ylabel('Acceleration (deg/s^2)')
ylim([-600 600])
subplot(4,1,4)
plot(tMid, jMid(:,1))
ylim([-50000 50000])
ylabel('(deg/s^3)'), xlabel('Time (s)')

%subplot(2,1,2)
%plot(SMid_plot_xaxis, SMid_plot_yaxis, SMid_plot_xaxis, IMU_bestfit, SMid_plot_xaxis, IMU_corrected_func, SMid_plot_xaxis, Kin_bestfit)
%xlim([0 Vic_time])
%legend('Uncorrected', 'Uncorrected Mean', 'Corrected', 'Corrected Mean')
% xlim([0 SMid_time])
% ylabel('y'),xlabel('Time (s)')


%SMid_plot_yaxis = resample(SMid_plot_yaxis,length(Vic_plot_yaxis),length(SMid_plot_yaxis));
%IMU_corrected_func = resample(IMU_corrected_func,length(Vic_plot_yaxis),length(IMU_corrected_func));
%SMid_plot_xaxis = resample(SMid_plot_xaxis,length(Vic_plot_xaxis),length(SMid_plot_xaxis));


%Kin_plot_y = resample(Kin_plot_y,length(Vic_plot_yaxis),length(Kin_plot_y));
%Kin_plot_time = resample(Kin_plot_time,length(Vic_plot_xaxis),length(Kin_plot_time));

%angleRMSE_IMU = sqrt(mean((Vic_plot_yaxis - SMid_plot_yaxis).^2));

%angleRMSE_Corrected = sqrt(mean((Vic_plot_yaxis - IMU_corrected_func).^2));
%angleRMSE_Kin = sqrt(mean((Vic_plot_yaxis - Kin_plot_y).^2));