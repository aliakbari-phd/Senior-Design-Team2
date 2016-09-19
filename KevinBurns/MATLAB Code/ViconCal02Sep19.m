filename = 'Ben_Johnston Cal 02.csv';
V_Base = xlsread(filename, 'C18:E3116');
V_Upper = xlsread(filename, 'I12:Z4612');

V_Base_Zeroed_X = V_Base(:,1)-V_Base(1,1);
V_Base_Zeroed_Y = V_Base(:,2)-V_Base(1,2);
V_Base_Zeroed_Z = V_Base(:,3)-V_Base(1,3);

V_Base_Zero(:,1) = V_Base_Zeroed_X;
V_Base_Zero(:,2) = V_Base_Zeroed_Y;
V_Base_Zero(:,3) = V_Base_Zeroed_Z;

Maximum = max(abs(V_Base_Zero));

MaxVector = transpose(Maximum);

True_Max = max(MaxVector);

V_BaseNorm_X = V_Base_Zeroed_X./True_Max;
V_BaseNorm_Y = V_Base_Zeroed_Y./True_Max;
V_BaseNorm_Z = V_Base_Zeroed_Z./True_Max;

% V_Base_Normalized = V_Base_Zeroed_X./V_Base_Zeroed.max;

subplot(3,1,1)
plot(V_BaseNorm_X)
title('Vicon X-Direction')

subplot(3,1,2)
plot(V_BaseNorm_Y)
title('Vicon Y-Direction')
ylabel('Normalized Position')
ylim([-1 1])

subplot(3,1,3)
plot(V_BaseNorm_Z)
title('Vicon Z-Direction')
xlabel('Captures')
