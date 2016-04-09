velZ = GyroZ ./ 32.75;        
%velZ = abs(velZ);             %ensures positive results

t = 1:length(velZ);
t = t./200;                      %cycles to seconds
accel = diff(velZ);             % vel to accel 
accel = [0;accel];
jerk = diff(accel);             %accel to jerk
jerk = [0;jerk];
pos_trapz = cumtrapz(t,velZ);     % vel to pos
angular_distance = trapz(t,velZ)
final_t = t(end)
mean_vel = mean(velZ)

%best fit code
p1 = polyfit(transpose(t),velZ,1);
v1 = polyval(p1,t);

%plotting
set(gcf,'color','white')
subplot(5,1,1)
plot (t,velZ)
ylabel('vel'), xlabel('t')

subplot(5,1,2)
plot (t,v1)
ylabel('best fit vel 1'), xlabel('t')

subplot(5,1,3)
plot(t,pos_trapz)
ylabel('pos'), xlabel ('t')

subplot(5,1,4)
plot (t,accel)
ylabel('accel'), xlabel('t')

subplot(5,1,5)
plot (t,jerk)
ylabel('jerk'), xlabel('t')






