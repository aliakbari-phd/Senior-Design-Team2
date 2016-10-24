filename = 'AngularData.txt';
delimiterIn = ' ';
headerlinesIn = 1;
AngularData = importdata(filename, delimiterIn, headerlinesIn);

Angle = AngularData.data(:,1);
AngularVelocity = AngularData.data(:,2);
AngularAcceleration = AngularData.data(:,3);
AngularJerk = AngularData.data(:,4);
Time = AngularData.data(:,5);

Angle2 = cumtrapz(Time,AngularVelocity);
Angle2 = Angle2 + 90;
figure(1)
plot(Time, Angle)
title('Angle vs. Time')
ylabel('Angle (deg)'),xlabel('Time (s)')
figure(2)
plot(Time, AngularVelocity)
title('Angular Velocity vs. Time')
ylabel('Angular Velocity (deg/s)'),xlabel('Time (s)')
figure(3)
plot(Time, AngularAcceleration)
title('Angular Acceleration vs. Time')
ylabel('Angular Acceleration (deg/s^2)'),xlabel('Time (s)')
figure(4)
plot(Time, AngularJerk)
title('Angular Jerkn vs. Time')
ylabel('Angular Jerk (deg/s^3)'),xlabel('Time (s)')
figure(5)
plot(Time, Angle2)
title('Matlab cumtrapz')
ylabel('Angle (deg)'),xlabel('Time (s)')