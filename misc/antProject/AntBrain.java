import java.awt.Graphics2D;
import java.awt.Color;
import java.awt.geom.Rectangle2D;
import java.util.*;




class AntBrain{

	public AntBrain(){}

	public int think(ProtectedAnt ant, Random rng){
		if(!ant.hasFood()){
			if(ant.checkFood(0)){
				ant.gatherFood();
				ant.setDynamicMemory(1);
			}
		}

		if(ant.getStamina()<(ant.getMaxStamina()/2+1)){
			ant.setDynamicMemory(1);
		}

		if(ant.getDynamicMemory() == 1){
			return 5;
		}

		return (rng.nextInt(4)+1);

	}
}


