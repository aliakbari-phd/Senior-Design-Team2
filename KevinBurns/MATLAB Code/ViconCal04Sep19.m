filename = 'Ben_Johnston Cal 01.csv';
V_Base = xlsread(filename, 'C12:E4612');
V_Upper = xlsread(filename, 'I12:Z4612');

V_Base_Zeroed_X = V_Base(:,1)-V_Base(1,1);
V_Base_Zeroed_Y = V_Base(:,2)-V_Base(1,2);
V_Base_Zeroed_Z = V_Base(:,3)-V_Base(1,3);

X_max = max(abs(V_Base_Zeroed_X));
Y_max = max(abs(V_Base_Zeroed_Y));
Z_max = max(abs(V_Base_Zeroed_Z));

V_BaseNorm_X = V_Base_Zeroed_X./X_max;
V_BaseNorm_Y = V_Base_Zeroed_Y./Y_max;
V_BaseNorm_Z = V_Base_Zeroed_Z./Z_max;

% V_Base_Normalized = V_Base_Zeroed_X./V_Base_Zeroed.max;

subplot(3,1,1)
plot(V_BaseNorm_X)
title('Vicon X-Direction')

subplot(3,1,2)
plot(V_BaseNorm_Y)
title('Vicon Y-Direction')
ylabel('Normalized Position')

subplot(3,1,3)
plot(V_BaseNorm_Z)
title('Vicon Z-Direction')
xlabel('Captures')
