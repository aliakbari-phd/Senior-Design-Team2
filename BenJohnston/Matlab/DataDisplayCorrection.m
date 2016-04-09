%NOTE:
%Need to import GyroZ and Ltime columns from Bapgui
filename = '2 hour stationary plugged 2.txt';
delimiterIn = '\t';
headerlinesIn = 1;
A = importdata(filename, delimiterIn, headerlinesIn);
%Program reports data using the z-axis of the gyroscope
%including angular position, velocity, acceleration and jerk,
%includes correction factor

%import data
velZ = (A.data(:,6))./32.75;          %Gyroscope correction factor
Ltime = (A.data(:,13));
t = transpose((Ltime-Ltime(1))./1000);     %relative to start time, ms to s

accel = diff(velZ);             % vel to accel 
accel = [0;accel];
jerk = diff(accel);             %accel to jerk
jerk = [0;jerk];
distance = cumtrapz(t,velZ);     % vel to distance

%best fit code
p1 = polyfit(transpose(t),velZ,1);
v1 = transpose(polyval(p1,t));      %best fit line of velocity data

%correction
vdelta = v1 - v1(1);            %discrepancy between velocity and best fit

%{
%*******low pass filter*****
x_filter = designfilt('lowpassiir','FilterOrder',8,...
            'PassbandFrequency',150,'PassbandRipple',0.5,...
            'SampleRate',200e3);
dataIn = velZ;
velZ = filter(x_filter,dataIn);
%}
vel_corrected = velZ - abs(vdelta);     %subtract discprepancy from velocity
dist_corrected = cumtrapz(t,vel_corrected);  %find absolute distance

%p2 = polyfit(transpose(t),vel_corrected,1);
%v2 = transpose(polyval(0.13,t));

%frequency consistency
delta_t = 0;
for n=2:length(t)
    delta_t(end+1)=t(n)-t(n-1);
end;

%results:
result_drift = abs(v1(end)-v1(1)); %difference in begin to end of best fit velocity
result_duration = t(end);
result_Fs = length(velZ)/t(end);       %Fs = sampling frequency
result_corrected_distance = dist_corrected(end);     %report final corrected distance
result_distance = distance(end);    %report final distance
result_mean_vel = abs(mean(velZ));   %report mean velocity

%plotting
set(gcf,'color','white')
subplot(3,1,1)
plot (t,velZ)
title('Angular Velocity vs Time')
ylabel('Velocity (deg/s)'), %xlabel('Time (s)')
xlim([0 t(end)])

subplot(3,1,2)
plot (t,v1)
title('Best Fit Angular Velocity vs Time')
ylabel('Best Fit Velocity (deg/s)'), %xlabel('Time (s)')
xlim([0 t(end)])

subplot(3,1,3)
plot(t,distance)
title('Angular Distance vs Time')
ylabel('Distance(deg)'), xlabel ('Time (s)')
xlim([0 t(end)])

%subplot(4,1,4)
%plot(t,transpose(delta_t))
%ylabel('delta t (s)'), xlabel ('time (s)')