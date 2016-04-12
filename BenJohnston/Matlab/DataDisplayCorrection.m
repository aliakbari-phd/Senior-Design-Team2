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
a_velocity(:,1) = (A.data(:,4))./32.75;  
a_velocity(:,2) = (A.data(:,5))./32.75;  
a_velocity(:,3) = (A.data(:,6))./32.75;          %Gyroscope correction factor
Ltime = (A.data(:,13));
t = transpose((Ltime-Ltime(1))./1000);     %relative to start time, ms to s

%remove first one percent of data
onepercent = round(0.01*length(a_velocity));
%a_velocity(1:onepercent,:)=[];
%t(1:onepercent,:)=[];


%*******low pass filter*****
x_filter = designfilt('lowpassiir','FilterOrder',3,...
            'PassbandFrequency',10e3,'PassbandRipple',0.5,...
            'SampleRate',200e3);
a_velocity = filter(x_filter,a_velocity);

%best fit code
t3(:,1) = transpose(t);
t3(:,2) = transpose(t);
t3(:,3) = transpose(t);
p1 = polyfit(t3,a_velocity,1);
c_velocity = transpose(polyval(p1,t));      %best fit line of velocity data

%correction factor (eliminate offset "constant")
mean_x = mean(a_velocity(:,1));
mean_y = mean(a_velocity(:,2));
mean_z = mean(a_velocity(:,3));
a_velocity(:,1)=a_velocity(:,1)-mean_x;
a_velocity(:,2)=a_velocity(:,2)-mean_y;
%a_velocity(:,3)=a_velocity(:,3)-mean_z;

%zero testing
%a_velocity(:,1)=zeros(size(a_velocity(:,1)));
%a_velocity(:,2)=zeros(size(a_velocity(:,2)));
%a_velocity(:,3)=zeros(size(a_velocity(:,3)));

%differentiation and integration
a_acceleration = diff(a_velocity);             % vel to accel 
a_acceleration = [0,[1 3];a_acceleration];
a_jerk = diff(a_acceleration);             %accel to jerk
a_jerk = [0,[1 3];a_jerk];
a_position = trapz(t,a_velocity);
a_distance = cumtrapz(t,a_velocity);     % vel to distance

%angular distance to linear position
position(:,1) = cosd(a_distance(:,3))+sind(a_distance(:,2));
position(:,2) = -1+cosd(a_distance(:,1))+sind(a_distance(:,3));
position(:,3) = -1+cosd(a_distance(:,2))+sind(a_distance(:,1));

%plotting
set(gcf,'color','white')
subplot(2,1,1)
plot3(position(:,1),position(:,2),position(:,3))
title('3-D Linear Position based on Relative Angular Velocity')
xlabel('x Position'),ylabel('y Position'),zlabel('z Position')
zlim([-1 1])
grid on
savefig('3d_grid.fig')


subplot(2,1,2)
plot(t,a_velocity(:,1))
title('x Axis Angular Velocity')
ylabel('Angular Velocity (deg/s)'),xlabel('Time (s)')
%ylim([-.1 .1])

%results
abs_distance = cumtrapz(t,abs(a_velocity));

result_duration = t(end);                   %report duration of test
result_Fs = length(a_velocity)/t(end);       %Fs = sampling frequency
result_xdistance = abs(abs_distance(end,1));      %report final distance
result_ydistance = abs(abs_distance(end,2));
result_zdistance = abs(abs_distance(end,3));  
result_drift = abs(a_velocity(end,1)-a_velocity(onepercent,1));
result_bfdrift = abs(c_velocity(end)-c_velocity(1)); %difference in begin to end of best fit velocity
result_mean_vel = abs(mean(a_velocity(:,3)));   %report mean velocity


%subplot(3,1,3)
%plot(t,c_velocity(:,1))
%ylim([-2 0])



%{
%correction
vdelta = v1 - v1(1);            %discrepancy between velocity and best fit


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