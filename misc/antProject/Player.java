import java.awt.Graphics2D;
import java.awt.Color;
import java.awt.geom.Rectangle2D;
import java.util.*;

class Player{
	private int x;
	private int y;
	private int food;
	private int[] color;
	private int playerNumber;
	private ArrayList<Ant> ants;

	public Player(int[] color, int playerNumber, int x, int y, int food) {
		this.x = x;
		this.y = y;
		this.color = color;
		this.food = food;
		this.playerNumber = playerNumber;
		this.ants = new ArrayList<Ant>();
	}

	// set functions
	//public void setX(int x){this.x = x;}
	//public void setY(int y){this.y = y;}
	//public void setPlayerNumber(int playerNumber){this.playerNumber = playerNumber;}

	// get functions
	public int getX(){return x;}
	public int getY(){return y;}
	public int getPlayerNumber(){return playerNumber;}

	// draw everything of this playuer on the board
	public void draw(int[][][] picture){
		//ig2.setPaint(Color.blue);

	    Iterator<Ant> iterator = ants.iterator();
		while (iterator.hasNext()){
			iterator.next().draw(picture, color);
		}
		//ig2.setPaint(Color.red);
	    //ig2.fill(new Rectangle2D.Double(x*8, y*8, 8, 8));
	    picture[x][y][0]+=color[0];
	    picture[x][y][1]+=color[1];
	    picture[x][y][2]+=color[2];
	}

	public int colonySize(){
		return ants.size();
	}

	// execute a turn
	public void turn(Referee referee){
		//Referee referee2 = new Referee(10);
		//referee2.checkFood(0,0);
		if(food>=5){
			ants.add(new Ant(x,y, 100, referee));
			food -= 5;
		}
		Iterator<Ant> iterator = ants.iterator();
		while (iterator.hasNext()){
			Ant ant = iterator.next();

			// do asnt loop
			ant.move(ant.turn());

			// check of ant has returned and if so how much food
			if(ant.isDead())
			{
				referee.addFood(ant.getX(), ant.getY(), 5);
				iterator.remove();
			}
			ant.applyFeromone(referee);
			food += ant.endTurn(x, y);
		}

		//System.out.printf("a= %d\n", ants.size());
	}
}

class Move{
	// stand = 0
	// up = 1
	// right = 2
	// down = 3
	// left = 4
	static public int xy2dir(int x, int y){
		if(x==-1)
			return 1;
		else if(x==1)
			return 3;
		else if(y==1)
			return 2;
		else if(y==-1)
			return 4;
		else if(x!=0 || y!=0)
			System.out.printf("direction error");
		return 0;
	}

	static public int dir2x(int dir){
		if(dir==2)
			return 1;
		else if(dir==4)
			return -1;
		else
			return 0;	
	}

	static public int dir2y(int dir){
		if(dir==1)
			return -1;
		else if(dir==3)
			return 1;
		else
			return 0;	
	}

	static public int swapDir(int dir){
		if(dir == 1)
			return 3;
		else if(dir == 2)
			return 4;
		else if(dir == 3)
			return 1;
		else if(dir == 4)
			return 2;
		else
			return 0;
	}
}

class Ant{
	private int x;
	private int y;
	private boolean dead = false;
	private ArrayList<Integer> path = new ArrayList<Integer>();
	private Random rng = new Random();
	private boolean returning = false;
	private int food = 0;
	private int maxFood = 1;
	private int stamina, maxStamina;
	private double fermoneDosis = 0;
	private Referee referee;

	public Ant(int x, int y, int maxStamina, Referee referee){
		this.x = x;
		this.y = y;
		this.stamina = maxStamina;
		this.maxStamina = maxStamina;
		this.referee = referee;
	}

	public int getStamina(){return stamina;}
	public int getMaxStamina(){return maxStamina;}

	public void setFeromoneDosis(double feromoneDosis){
		this.fermoneDosis = feromoneDosis;
	}

	public void applyFeromone(Referee referee)
	{
		referee.addFeromones(x,y,fermoneDosis);
	}

	//public void setX(int x){this.x = x;}
	//public void setY(int y){this.y = y;}
	public int getX(){return x;}
	public int getY(){return y;}

	public int endTurn(int x, int y){
		stamina--;


		//check if returned to home base
		if(this.x == x && this.y == y)
		{
			returning = false;
			stamina = maxStamina;
			path.removeAll(path);
			setFeromoneDosis(0);
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

		if(stamina<1)
			System.out.printf("D");

		return (stamina<1);
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
			return;
		}

		// border protection. dont move if going out of bounds
		if(x==0 && dir==4)
			return;
		if(x==63 && dir==2)
			return;
		if(y==0 && dir==1)
			return;
		if(y==63 && dir==3)
			return;

		// if 1,2,3,4 move in specified direction
		path.add(dir);
		x += Move.dir2x(dir);
		y += Move.dir2y(dir);
		return;
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

	public int turn(){
		/////////// PLAYER ADDED FUNCTIONAILTY

		// calculate incentives
		double inc[] = {1,1,1,1,1};
		int dir = 0;	

		
		if(!hasFood()){
			if(referee.checkFood(x,y)){
				gatherFood();
				returning = true;
				setFeromoneDosis(256/(getMaxStamina()-getStamina()+1));
			}
		}
		

		// return if almost out of juice
		if(path.size()>48){
			returning = true;
		}


		if(returning){
			return 5;
		}

		// border protection
		if(x==0)
			inc[4]=0;
		if(x==63)
			inc[2]=0;
		if(y==0)
			inc[1]=0;
		if(y==63)
			inc[3]=0;
		
		// penalty for staying stationary
		inc[0] *= 0.02;

		
		// add value of feromones
		for(int i=1; i<5; i++){
			if(inc[i]!=0)
				inc[i] += referee.getFeromones(x+Move.dir2x(i), y+Move.dir2y(i));
		}

		if(path.size()>0){
			// incentive for moving forward
			inc[path.get(path.size()-1)] *= 4;

			// penalty for going back
			inc[Move.swapDir(path.get(path.size()-1))] *= 0.02;
		}

		
		// when incentives are generated pick a probabilistic random one
		double tmp = (inc[0]+inc[1]+inc[2]+inc[3]+inc[4])*rng.nextDouble();
		for(int i=0; i<5; i++){
			tmp -=inc[i];
			if(tmp<=0){
				dir = i;
				break;
			}
		}
		

		// dir now stores the new direction. Update everything
		return dir;
		}
}

class protectedAnt{
	private Ant ant;

	public protectedAnt(Ant ant){
		this.ant = ant;
	}

	public void setAnt(Ant ant){this.ant = ant;}
	public int getStamina(){return ant.getStamina();}
	public int getMaxStamina(){return ant.getMaxStamina();}
	public boolean hasFood(){return ant.hasFood();}
	public boolean checkFood(int dir){return ant.checkFood(dir);}
	public void gatherFood(int dir){ant.gatherFood();}
}


class antBrain{
	static public int think(protectedAnt ant){
		return 0;
	}
}


class Referee{
	private int w,h;
	private ArrayList<int[]> food;
	private Random rng = new Random();
	private double[][] feromones;


	public Referee(int foodAmount, int w, int h){
		this.w = w;
		this.h = h;
		food = new ArrayList<int[]>();
		rng.setSeed(0);
		feromones = new double[w][h];
		for(int i=0; i<w; i++){
			for(int j=0; j<h; j++){
				feromones[i][j] = 0;
			}
		}

		for(int i=0; i<foodAmount; i++){
			addFood(rng.nextInt(w), rng.nextInt(h), 30);
		}
	}

	public void addFood(int x, int y, int amount){
		food.add(new int[]{x, y, amount});
	}

	public void addFeromones(int x, int y, double dosis){
		feromones[x][y] += dosis;
	}

	public double getFeromones(int x, int y){
		return feromones[x][y];
	}

	public int gatherFood(int x, int y){
		for(int i=0; i<food.size(); i++){
			if(food.get(i)[0]==x && food.get(i)[1]==y){
				
				food.get(i)[2]--;
				// gradually remove food
				if(food.get(i)[2]<1){
					food.remove(i);
				}
				return 1;
			}
		}
		return 0;
	}

	public boolean checkFood(int x, int y){
		for(int i=0; i<food.size(); i++){
			if(food.get(i)[0]==x && food.get(i)[1]==y){
				return true;
			}
		}
		return false;
	}

	public void draw(int[][][] picture){
		int[] coor;
		//ig2.setPaint(Color.green);
	    Iterator<int[]> iterator = food.iterator();
		while (iterator.hasNext()){
			coor = iterator.next();

			picture[coor[0]][coor[1]][0]+=70;
		    picture[coor[0]][coor[1]][1]+=70;
		    picture[coor[0]][coor[1]][2]+=70;
		}
	}

	public void turn(){
		for(int i=0; i<w; i++)
			for(int j=0; j<h; j++){
				feromones[i][j] *= 0.9;
			}
	}
}