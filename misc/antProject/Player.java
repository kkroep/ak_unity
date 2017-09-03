import java.awt.Graphics2D;
import java.awt.Color;
import java.awt.geom.Rectangle2D;
import java.util.*;

class Player{
	private int x;
	private int y;
	private String name;
	private int playerNumber;
	private ArrayList<Ant> ants;

	public Player(String name, int playerNumber, int x, int y) {
		this.x = x;
		this.y = y;
		this.name = name;
		this.playerNumber = playerNumber;
		this.ants = new ArrayList<Ant>();
	}

	// set functions
	public void setX(int x){this.x = x;}
	public void setY(int y){this.y = y;}
	public void setPlayerNumber(int playerNumber){this.playerNumber = playerNumber;}
	public void setName(String name){this.name = name;}

	// get functions
	public int getX(){return x;}
	public int getY(){return y;}
	public int getPlayerNumber(){return playerNumber;}
	public String getName(){return name;}

	// draw everything of this playuer on the board
	public void draw(int[][][] picture){
		//ig2.setPaint(Color.blue);

	    Iterator<Ant> iterator = ants.iterator();
		while (iterator.hasNext()){
			iterator.next().draw(picture);
		}
		//ig2.setPaint(Color.red);
	    //ig2.fill(new Rectangle2D.Double(x*8, y*8, 8, 8));
	    picture[x][y][0]=255;
	}

	// execute a turn
	public void turn(Referee referee){
		//Referee referee2 = new Referee(10);
		//referee2.checkFood(0,0);
		ants.add(new Ant(x,y));

		Iterator<Ant> iterator = ants.iterator();
		while (iterator.hasNext()){
			if(iterator.next().turn(referee)==1)
				iterator.remove();
		}

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
	private int qX; // queen x
	private int qY; // queen y
	private ArrayList<Integer> path;
	private Random rng;
	private boolean hasFood;
	private int pathLength;

	public Ant(int x, int y){
		this.x = x;
		this.y = y;
		this.qX = x;
		this.qY = y;
		this.pathLength = Integer.MAX_VALUE;
		hasFood = false;
		rng = new Random();
		path = new ArrayList<Integer>();
	}

	public void setX(int x){this.x = x;}
	public void setY(int y){this.y = y;}
	public int getX(){return x;}
	public int getY(){return y;}

	public void draw(int[][][] picture){
	    picture[x][y][2]+=50;
	    if(picture[x][y][2]<100)
	    	picture[x][y][2]=100;
	    else if(picture[x][y][2]>255)
	    	picture[x][y][2]=255;

	    if(!hasFood)
	    	return;
		picture[x][y][0]+=50;
	    if(picture[x][y][0]>255)
	    	picture[x][y][0]=255;

	}

	public int turn(Referee referee){
		// calculate incentives
		double inc[] = {1,1,1,1,1};
		int oX = x; // old x
		int oY = y; // old y
		int dir = 0;	


		if(path.size()>150){
			return 1;
		}

		// if at homebase, clean path and start over
		if(x == qX && y == qY){
			path.removeAll(path);
			if(hasFood){
				//System.out.printf("1");
				hasFood = false;
			}
		}else if(referee.checkFood(x,y)){
			hasFood = true;
			pathLength = path.size();
		}

		if(hasFood && path.size()>0){
			referee.addFeromones(x,y,256/pathLength);
			dir = Move.swapDir(path.get(path.size()-1));
			x += Move.dir2x(dir);
			y += Move.dir2y(dir);
			path.remove(path.size()-1);
			return 0;
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
			inc[path.get(path.size()-1)] *= 2;

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
		if(dir!=0)
			path.add(dir);
		x += Move.dir2x(dir);
		y += Move.dir2y(dir);
		//System.out.printf("(%d,%d)",x,y);

		return 0;
	}
}

class Referee{
	private int w,h;
	private ArrayList<int[]> food;
	private Random rng;
	private double[][] feromones;


	public Referee(int foodAmount, int w, int h){
		this.w = w;
		this.h = h;
		food = new ArrayList<int[]>();
		rng = new Random();
		feromones = new double[w][h];
		for(int i=0; i<w; i++){
			for(int j=0; j<h; j++){
				feromones[i][j] = 0;
			}
		}

		for(int i=0; i<foodAmount; i++){
			food.add(new int[]{rng.nextInt(w), rng.nextInt(h), 30});
		}
	}

	public void addFeromones(int x, int y, double dosis){
		feromones[x][y] += dosis;
	}

	public double getFeromones(int x, int y){
		return feromones[x][y];
	}

	public boolean checkFood(int x, int y){
		for(int i=0; i<food.size(); i++){
			if(food.get(i)[0]==x && food.get(i)[1]==y){
				
				// gradually remove food
				food.get(i)[2]-=1;
				if(food.get(i)[2]<=0)
					food.remove(i);
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
			picture[coor[0]][coor[1]][1]+=120;
		    if(picture[coor[0]][coor[1]][1]>255)
		    	picture[coor[0]][coor[1]][1]=255;
		}
	}

	public void turn(){
		for(int i=0; i<w; i++)
			for(int j=0; j<h; j++){
				feromones[i][j] *= 0.85;
			}
	}
}