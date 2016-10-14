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
Vic_t = Vic_frames./100;


%%%%% First Derivative for flex (Angular Velocity)
VPosderiv = diff(alpha_deg);
VTimderiv = diff(Vic_t);
VVel = VPosderiv./VTimderiv;

%%%%% Second Derivative for flex (Angular Acceleration)
VVelderiv = diff(VPosderiv);
VAcc = VVelderiv./VTimderiv(1:1549);

%%%%% Third Derivative for flex (Angular Jerk)
VAccderiv = diff(VVelderiv);
VJer = VAccderiv./VTimderiv(1:1548);

%%%%% Plotting
subplot(4,1,1)
plot(Vic_plot_xaxis,Vic_plot_yaxis)
xlim([0 Vic_time])
title('Angular Distance (deg)')
ylabel('Angle (degrees)'),xlabel('Time (s)')
legend('Vicon','IMU', 'Kinect')

subplot(4,1,2)
plot(Vic_frames(1:1550)./100,VVel)

subplot(4,1,3)
plot(Vic_frames(1:1549)./100,VAcc)
ylim([-11 11])

subplot (4,1,4)
plot(Vic_frames(1:1548)./100,VJer)
ylim([-14 14])

%%%%% Maximums
VVel_abs = abs(VVel);
VAcc_abs = abs(VAcc(2:1549));
VJer_abs = abs(VJer(2:1548));

VVel_max = max(VVel_abs)
VAcc_max = max(VAcc_abs)
VJer_max = max(VJer_abs)