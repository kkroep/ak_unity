import java.awt.Graphics2D;
import java.awt.Color;
import java.awt.geom.Rectangle2D;
import java.util.*;

class AntProperties{
	private int[] staticMemory = new int[]{0,0,0,0};
	private int dynamicMemory = 0; 

	private int health, maxHealth, damage, food, maxFood, stamina, maxStamina;

	public AntProperties(int maxHealth, int damage, int maxFood, int maxStamina){
		this.health = maxHealth;
		this.maxHealth = maxHealth;
		this.damage = damage;
		this.food = maxFood;
		this.maxFood = maxFood;
		this.stamina = maxStamina;
		this.maxStamina = maxStamina;
	}


	public void clearMemory(){
		staticMemory = new int[]{0,0,0,0};
		dynamicMemory = 0; 
	}

	public int getStamina(){return stamina;}
	public int getMaxStamina(){return maxStamina;}
	public void staminaRefill(){stamina = maxStamina;}

	public int calculateCost(){
		int cost = 10;



		// health cost. starts at 4, one food for one extra
		cost += (maxHealth-4);

		// damage cost
		cost += (damage-1)*4; 
		
		// food cost
		cost += maxFood*4;

		

		// make sure the ant is a valid one
		if(maxHealth<4 || damage<1 || maxFood<0 || maxStamina<50)
			cost = Integer.MAX_VALUE;

		return cost;
	}

	public int getDynamicMemory(){return dynamicMemory;}
	public void setDynamicMemory(int value){dynamicMemory = value;}
	public int getStaticMemory(int index)
	{	
		if(index>=0 && index < 4)
			return staticMemory[index];
		return 0;
	}
	public void setStaticMemory(int index, int value)
	{	
		if(index>=0 && index < 4)
			 staticMemory[index] = value;
	}

	public void turn(){stamina--;}
}
