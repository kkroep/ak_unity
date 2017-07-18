

colorspec = {[0.4 0 0.8]; [0.4 0.8 0]; [0.4 0.7 0.7]; ...
  [0 0.4 0.8]; [0.8 0.4 0]; [0.7 0.4 0.7]; ...
  [0.8 0 0.4]; [0 0.8 0.4]; [0.7 0.7 0.4]; ...
  [0 0 0.7]; [0 0.7 0]; [0.7 0 0]};

colorspec = {...
[0.0 0 1.0]; ...
[0.2 0 0.8]; ... 
[0.4 0 0.6]; ... 
[0.6 0 0.4]; ... 
[0.8 0 0.2]; ... 
[1.0 0 0.0]; ... 
};

graphics_toolkit gnuplot;
figure ("visible", "off");


PPS = 1e5; % [Hz]
deadTimeSteps = 5; % multiple of stepSize

endTime = 1e-2; % [s]
stepSize = 1e-6; % [s]
waitingTime = 0:stepSize:endTime;


iterations = 1e4; % [-]


values = zeros(1,length(waitingTime));

P_hit = 1-poisspdf(0, PPS*stepSize) % chance that one or more photons arrive during t_step


for i=1:iterations
    new_value = zeros(1,length(waitingTime));
    current = 0 ;
    dead = 0;
    for t=1:length(waitingTime)
        if rand<P_hit || dead<=0
            current = mod(current+1,2);  % flip the current value
            dead = deadTimeSteps;
        end
        new_value(t) = current;
        dead = dead-1;
    end
    values = values + new_value;
end

plot(waitingTime, values, 'Linewidth', 2, 'Color', colorspec{mod(1,6)+1});

%hold off;

%plot(C1(:,1),C1(:,4), 'Color', colorspec{mod(i,12)+1});
%axis([C1(1,1) C1(end,1) min(min(C1))*1.1 max(max(C1))*1.1]);
%xlim([min(C1(:,2)), max(C1(:,2))]);
%ylim([1e-1, 1e2]);
%xlabel('packetrate [s]', 'fontsize', 14);
%ylabel('RMSE [-]', 'fontsize', 14);
%set(gca, 'FontSize', 12)


%legend(legendstring, 'Location', 'northeastoutside');
%title('error to packetrate for varying delays. Packetsize 1', 'fontsize', 14);
%
print('-dpdf', '-color', fullfile(pwd, 'sneep.pdf'));
%print('-deps', '-color', fullfile(pwd, 'lineplot.eps'));

