velX = GyroX ./ 32.75;      %correction factor
velY = GyroY ./ 32.75;
velZ = GyroZ ./ 32.75;
velX = abs(velX);
velY = abs(velY);
velZ = abs(velZ);
t = 1:length(GyroX);
t = t./200;                 %cycles to seconds

subplot(4,1,1)
plot(t,velX)
ylabel('Vel X'), xlabel ('t')

subplot(4,1,2)
plot(t,velY)
ylabel('Vel Y'), xlabel ('t')

subplot(4,1,3)
plot(t,velZ)
ylabel('Vel Z'), xlabel ('t')

subplot (4,1,4)
plot(t,velX)
hold on
plot(t,velY)
plot(t,velZ)
ylabel('Angular Velocity'), xlabel('t')
hold off