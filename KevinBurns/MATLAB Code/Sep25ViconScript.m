filename = 'Ben_Johnston Cal 01.csv';
V_Data = xlsread(filename, 'A12:N1582');

pnts_base(:,1) = V_Data(:,3);           %base points
pnts_base(:,2) = V_Data(:,4);
pnts_base(:,3) = V_Data(:,5);

pnts_upper(:,1) = V_Data(:,9);          %upper points
pnts_upper(:,2) = V_Data(:,10);
pnts_upper(:,3) = V_Data(:,11);

v_zunit = ([0 0 1]);            %create z unit vector
Vic_frames = V_Data(:,1);


v_pntpnt = pnts_upper - pnts_base;      %point to point vector

n=1;
i=1;
v_length = size(v_pntpnt);
l = v_length(1,1);
while i<l
v_pnt_norm = v_pntpnt(i,:)./norm(v_pntpnt(i,:));
i = i+1;
theta(i,:) = acos(dot(v_pnt_norm,v_zunit));
end
alpha = (pi/2)-theta;
alpha_deg = alpha.*(180/pi);

[Vic_pks, Vic_locs] = findpeaks(alpha_deg, 'MinPeakProminence', .5);

Vic_peak_beg = Vic_locs(1)-200;
Vic_peak_end = Vic_locs(end);

Frames_used = Vic_frames(Vic_peak_end)-Vic_frames(Vic_peak_beg);

% theta = atan2(norm(cross(v_pntpnt, v_zunit)), dot(v_pntpnt, v_zunit));

Vic_time = (Frames_used)/100;
Vic_plot_yaxis = alpha_deg(Vic_peak_beg:Vic_peak_end);
Vic_plot_xaxis = 0:Vic_time/(Frames_used):Vic_time;

% plot(Vic_frames(Vic_peak_beg:Vic_peak_end),alpha_deg(Vic_peak_beg:Vic_peak_end), Vic_frames(Vic_locs), Vic_pks, 'or')

plot(Vic_plot_xaxis,Vic_plot_yaxis)
xlim([0 Vic_time])

result = norm(v_pntpnt);

% hold on
% plot(V_Data(:,1),V_Data(:,11))
% hold on
% plot(V_Data(:,1),V_Data(:,5))
% hold on
% plot(V_Data(:,1),V_Data(:,4))
% hold on
% plot(V_Data(:,1),V_Data(:,10))
% hold on
% plot(V_Data(:,1),V_Data(:,3))
% hold on
% plot(V_Data(:,1),V_Data(:,9))