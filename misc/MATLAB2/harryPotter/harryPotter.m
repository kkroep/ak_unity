clc;
close all;
clear;

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

%graphics_toolkit gnuplot;
%figure ("visible", "off");

PPS = [5e3 5e4 5e5]; % [Hz]

endTime = 60e-6; % [s]
stepSize = 2e-9; % [s]
deadTime = 10e-6; % [s]
deadTimeSteps = round(deadTime/stepSize);

%endTime = 2e-3; % [s]
%stepSize = 1e-6; % [s]

waitingTime = 0:stepSize:endTime;
iterations = 25e3; % [-]
values = zeros(1,length(waitingTime));
legendString = {};

for pps = PPS
    P_hit = 1-poisspdf(0, pps*stepSize) % chance that one or more photons arrive during t_stepT
    for dTime = deadTimeSteps
        for j=1:10
            fprintf('%d0\n', j);
            for i=1:iterations/10
                new_value = zeros(1,length(waitingTime));
                current = 0;
                dead = 0;
                for t=1:length(waitingTime)
                    if dead>0
                        dead = dead - 1;
                    elseif rand<P_hit
                        current = mod(current+1,2);  % flip the current value                
                        dead = dTime;
                    end
                    new_value(t) = current;
                    dead = dead - 1;
                end
                values = values + new_value;
            end
        end
    values = values./iterations;
    %plot(waitingTime.*1e6, values);
    filename = sprintf('pps-%d_dead-%dns.csv', pps, 1e9*stepSize*deadTimeSteps); 
    
    fid = fopen(filename, 'w');
    fprintf(fid, 'time values\n');
    fclose(fid);
    
    waitingTimePrint = waitingTime(1:100:length(waitingTime))';
    valuesPrint = values(1:100:length(values))';
    
    dlmwrite(filename, [waitingTimePrint valuesPrint], '-append','Delimiter', ' ');
    %legendString{end+1} = sprintf('PPS = %d', pps);
    end
end

%plot(waitingTime*1e6, 0.5*ones(1,length(values)), 'k');
%hold off;

%plot(C1(:,1),C1(:,4), 'Color', colorspec{mod(i,12)+1});
%axis([C1(1,1) C1(end,1) min(min(C1))*1.1 max(max(C1))*1.1]);
%xlim([waitingTime(1)*1e6, waitingTime(end)*1e6]);
%ylim([1e-1, 1e2]);
%xlabel('time [us]', 'fontsize', 14);
%ylabel('probability of measuring ''1''', 'fontsize', 14);
%set(gca, 'FontSize', 12)


%legend(legendString, 'Location', 'northeast');
%title(sprintf('deadtime = %d us', 1e6*stepSize*deadTimeSteps));
%
%print('-dpdf', '-color', fullfile(pwd, 'sneep.pdf'));
%print('-deps', '-color', fullfile(pwd, 'lineplot.eps'));