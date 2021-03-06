import java.awt.Graphics2D;
import java.awt.Color;
import java.awt.geom.Rectangle2D;
import java.util.*;

class Ant{
	private int x;
	private int y;
	private boolean dead = false;
	private ArrayList<Integer> path = new ArrayList<Integer>();
	private Random rng = new Random();
	private int food = 0;
	private int maxFood = 1;
	private int stamina, maxStamina;
	private double fermoneDosis = 0;
	private Referee referee;
	private int previousDir = 0;
	private AntProperties antProps;
	private AntBrain antBrain;
	private int playerNumber;

	// this is teh memory of the ants. One can be set only by the queen, but read by both, 
	// the second one can be altered by both queen and ant
	private int[] staticMemory = new int[]{0,0,0,0};
	private int dynamicMemory = 0;


	public Ant(int x, int y, Referee referee, AntBrain antBrain, int playerNumber, int maxHealth, int damage, int maxFood, int maxStamina){
		this.x = x;
		this.y = y;
		this.referee = referee;
		this.playerNumber = playerNumber;
		this.antBrain = antBrain;
		this.antProps = new AntProperties(maxHealth, damage, maxFood, maxStamina);
	}

	public int getDynamicMemory(){return antProps.getDynamicMemory();}
	public int getStaticMemory(int index){return antProps.getStaticMemory(index);}
	public void setDynamicMemory(int value){antProps.setDynamicMemory(value);}
	public void clearMemory(){antProps.clearMemory();}

	public int getPreviousDir(){return previousDir;}
	public int getStamina(){return antProps.getStamina();}
	public int getMaxStamina(){return antProps.getMaxStamina();}

	public double getFeromones(int dir){return referee.getFeromones(x+Move.dir2x(dir), y+Move.dir2y(dir), playerNumber);}
	public void setFeromoneDosis(double feromoneDosis){
		this.fermoneDosis = feromoneDosis;
	}

	public void applyFeromone(Referee referee)
	{
		referee.addFeromones(x,y, playerNumber, fermoneDosis);
	}


	public boolean hasFood(){return food>0;}
	public int getFood(){return food;}
	public void gatherFood(){addFood(referee.gatherFood(x, y));}

	public void addFood(int amount){
		food += amount;

		// can only carry so much
		if(food>maxFood)
			food=maxFood;
	}

	public boolean checkFood(int dir){
		if(dir<0 || dir>4)
			dir=0;
		return referee.checkFood(x+Move.dir2x(dir), y+Move.dir2y(dir));
	}

	//public void setX(int x){this.x = x;}
	//public void setY(int y){this.y = y;}
	public int getX(){return x;}
	public int getY(){return y;}

	public int endTurn(int x, int y){
		antProps.turn();


		//check if returned to home base
		if(this.x == x && this.y == y)
		{
			antProps.staminaRefill();
			path.removeAll(path);
			setFeromoneDosis(0);
			antProps.clearMemory();
			if(hasFood())
			{
				int droppedFood = food;
				food = 0;
				return droppedFood;
			}
			return 0;
		}
		else
		{
			return 0;
		}

	}

	public boolean isDead(){
		//return false;
		/*if(stamina<1){
			return true;
		}
		else
			return false;*/

		if(antProps.getStamina()<1)
			System.out.printf("Death for P%d\n", playerNumber);

		return (antProps.getStamina()<1);
	}

	public void draw(int[][][] picture, int[] color){
	    picture[x][y][0]+=color[0]/8;
	    picture[x][y][1]+=color[1]/8;
	    picture[x][y][2]+=color[2]/8;

	    if(picture[x][y][0]<color[0]/8+color[0]/4){
			picture[x][y][0]+=color[0]/8+color[0]/4;
		    picture[x][y][1]+=color[1]/8+color[1]/4;
		    picture[x][y][2]+=color[2]/8+color[2]/4;
	    }

	    if(!hasFood())
	    	return;
		picture[x][y][0]+=35;
		picture[x][y][1]+=35;
		picture[x][y][2]+=35;
	}

	// 0 = stay put
	// 1 = up
	// 2 = right
	// 3 = down
	// 4 = left
	// 5 = reverse
	public void move(int dir){
		// if standing still or invalid instruction, stand still
		if(dir == 0)
			return;

		// if 5, walk back
		if(dir == 5){
			if(path.size()<1)
				return;
			dir = Move.swapDir(path.get(path.size()-1));
			x += Move.dir2x(dir);
			y += Move.dir2y(dir);
			path.remove(path.size()-1);
			previousDir = dir;
			return;
		}

		// border protection. dont move if going out of bounds
		if(x==1 && dir==4)
			return;
		if(x==62 && dir==2)
			return;
		if(y==1 && dir==1)
			return;
		if(y==62 && dir==3)
			return;

		// if 1,2,3,4 move in specified direction
		path.add(dir);
		previousDir = dir;
		x += Move.dir2x(dir);
		y += Move.dir2y(dir);
		return;
	}

	//KeKroepes player1 = new KeKroepes();
	
	public int turn(){
		return antBrain.think(new ProtectedAnt(this), rng);
		//return 0;
	}
}


