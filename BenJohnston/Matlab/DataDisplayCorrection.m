%NOTE:
%Need to import GyroZ and Ltime columns from Bapgui
filename = '200 Hz 15 sec 01.txt';
delimiterIn = '\t';
headerlinesIn = 1;
A = importdata(filename, delimiterIn, headerlinesIn);
%Program reports data using the z-axis of the gyroscope
%including angular position, velocity, acceleration and jerk,
%includes correction factor

%import data
a_velocity(:,1) = (A.data(:,4))./(5895/pi);  
a_velocity(:,2) = (A.data(:,5))./(5895/pi);  
a_velocity(:,3) = (A.data(:,6))./(5895/pi);          %Gyroscope correction factor
Ltime = (A.data(:,13));
t = transpose((Ltime-Ltime(1))./1000);     %relative to start time, ms to s

mean_x = mean(a_velocity(:,1));
mean_y = mean(a_velocity(:,2));
%{
a_velocity(:,1)=a_velocity(:,1)-mean_x;
a_velocity(:,2)=a_velocity(:,2)-mean_y;
a_velocity(:,1)=zeros(size(a_velocity(:,1)));
a_velocity(:,2)=zeros(size(a_velocity(:,2)));
%}
a_acceleration = diff(a_velocity);             % vel to accel 
a_acceleration = [0,[1 3];a_acceleration];
a_jerk = diff(a_acceleration);             %accel to jerk
a_jerk = [0,[1 3];a_jerk];
a_position = trapz(t,a_velocity);
a_distance = cumtrapz(t,a_velocity);     % vel to distance

position(:,1) = cos(a_distance(:,3))+sin(a_distance(:,2));
position(:,2) = -1+cos(a_distance(:,1))+sin(a_distance(:,3));
position(:,3) = -1+cos(a_distance(:,2))+sin(a_distance(:,1));

set(gcf,'color','white')
subplot(2,1,1)
plot3(position(:,1),position(:,2),position(:,3))
xlabel('x'),ylabel('y'),zlabel('z')
zlim([-.25 0.25])

grid on
subplot(2,1,2)
plot(t,a_velocity(:,1))

%{
%best fit code
p1 = polyfit(transpose(t),velocity,1);
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
vel_corrected = velocity - abs(vdelta);     %subtract discprepancy from velocity
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
result_Fs = length(velocity)/t(end);       %Fs = sampling frequency
result_corrected_distance = dist_corrected(end);     %report final corrected distance
result_distance = distance(end);    %report final distance
result_mean_vel = abs(mean(velocity));   %report mean velocity

%plotting
set(gcf,'color','white')
subplot(3,1,1)
plot (t,velocity)
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
%}