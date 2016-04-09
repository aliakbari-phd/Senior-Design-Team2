GyroZ = GyroZ ./ 32.75;        % comment out if corrected once already
GyroZ = abs(GyroZ);             %ensures positive results
t = 1:length(GyroZ);
t = t./200; 

GyroZ1 = GyroZ1 ./ 32.75;        % comment out if corrected once already
GyroZ1 = abs(GyroZ1);             %ensures positive results
t1 = 1:length(GyroZ1);
t1 = t1./200; 

GyroZ2 = GyroZ2 ./ 32.75;        % comment out if corrected once already
GyroZ2 = abs(GyroZ2);             %ensures positive results
t2 = 1:length(GyroZ2);
t2 = t2./200;

GyroZ3 = GyroZ3 ./ 32.75;        % comment out if corrected once already
GyroZ3 = abs(GyroZ3);             %ensures positive results
t3 = 1:length(GyroZ3);
t3 = t3./200;

subplot(4,1,1)
plot (t,GyroZ)
ylabel('vel 15'), xlabel('t'), ylim([250 300])

subplot(4,1,2)
plot (t1,GyroZ1)
ylabel('vel 30'), xlabel('t'), ylim([250 300])

subplot(4,1,3)
plot (t2,GyroZ2)
ylabel('vel 60'), xlabel('t'), ylim([250 300])

subplot(4,1,4)
plot (t3,GyroZ3)
ylabel('vel 300'), xlabel('t'), ylim([250 300])