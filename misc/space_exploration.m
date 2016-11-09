colorspec = {[0.4 0 0.8]; [0.4 0.8 0]; [0.4 0.7 0.7]; ...
  [0 0.4 0.8]; [0.8 0.4 0]; [0.7 0.4 0.7]; ...
  [0.8 0 0.4]; [0 0.8 0.4]; [0.7 0.7 0.4]; ...
  [0 0 0.7]; [0 0.7 0]; [0.7 0 0]};

graphics_toolkit gnuplot;


T_dead = 100e-9:200e-9:100e-8; % [s] deadtime of spads
D = 100e9; % [bit/s] throughput
SPAD_farm = 10.^(0.025*(1:100));


SPADS_tot = D*T_dead;
No_SCSs = SPADS_tot./(2*SPAD_farm');







plot(T_dead, SPADS_tot, 'LineWidth', 4, 'Color', colorspec{mod(1,12)+1});
%axis([-0.000001 0.00003 0.5 3.0]);
xlabel('deadtime [s]');
ylabel('No SPADs')
legend('100 Gbit/s','location', 'northeastoutside');
title('Lower bound on number of required SPADs');
print('-dpdf', '-color', fullfile(pwd,  'SPADS_tot.pdf'));

close;

hold on;

loglog(SPAD_farm(1,:), No_SCSs(:,1), 'LineWidth', 4, 'Color', colorspec{mod(1,12)+1});
loglog(SPAD_farm(1,:), No_SCSs(:,2), 'LineWidth', 4, 'Color', colorspec{mod(2,12)+1});
loglog(SPAD_farm(1,:), No_SCSs(:,3), 'LineWidth', 4, 'Color', colorspec{mod(3,12)+1});
loglog(SPAD_farm(1,:), No_SCSs(:,4), 'LineWidth', 4, 'Color', colorspec{mod(4,12)+1});

hold off;
%axis([-0.000001 0.00003 0.5 3.0]);
xlabel('SPADs in farm');
ylabel('SCSs in system')
legend(sprintf('T_{dead} = %d s', T_dead(1)),sprintf('T_{dead} = %d s', T_dead(2)),sprintf('T_{dead} = %d s', T_dead(3)),sprintf('T_{dead} = %d s', T_dead(4)),'location', 'northeastoutside');
title('Lower bound on number of required SPADs');
print('-dpdf', '-color', fullfile(pwd,  'No_SCSs.pdf'));
