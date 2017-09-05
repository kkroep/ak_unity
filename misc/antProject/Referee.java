import java.awt.Graphics2D;
import java.awt.Color;
import java.awt.geom.Rectangle2D;
import java.util.*;

class Referee{
	private int w,h;
	private ArrayList<int[]> food;
	private Random rng = new Random();
	private ArrayList<PlayerGrid> grid = new ArrayList<PlayerGrid>();


	public Referee(int foodAmount, int w, int h){
		this.w = w;
		this.h = h;
		food = new ArrayList<int[]>();
		rng.setSeed(0);
		grid.add(new PlayerGrid());
		grid.add(new PlayerGrid());


		for(int i=0; i<foodAmount; i++){
			addFood(rng.nextInt(w), rng.nextInt(h), 30);
		}
	}

	public void addFood(int x, int y, int amount){
		food.add(new int[]{x, y, amount});
	}

	public void addFeromones(int x, int y, int playerNumber, double dosis){
		grid.get(playerNumber).addActiveFeromones(x, y, 0, dosis);
	}

	public double getFeromones(int x, int y, int playerNumber){
		return grid.get(playerNumber).getActiveFeromones(x,y,0);
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
		grid.get(0).turn(0.9);
		grid.get(1).turn(0.9);
	}
}



class PlayerGrid{
	private double[][][] activeFeromones = new double[64][64][4];
	private double[][] enemyFeromones = new double[64][64];
	private int[][] damageGrid = new int[64][64];

	public PlayerGrid(){
		for(int i=0; i<64; i++)
			for(int j=0; j<64; j++)
			{
				enemyFeromones[i][j] = 0;
				for(int k=0; k<4; k++)
					activeFeromones[i][j][k] = 0;
			}
	}

	public void addActiveFeromones(int x, int y, int type, double dosis){	activeFeromones[x][y][type] += dosis;}
	public void addEnemyFeromones(int x, int y, double dosis){	enemyFeromones[x][y] += dosis;}
	public void addDamage(int x, int y, int amount){enemyFeromones[x][y] += amount;}


	public double getActiveFeromones(int x, int y, int type){return activeFeromones[x][y][type];}
	public double getEnemyFeromones(int x, int y){return enemyFeromones[x][y];}
	public int checkDamage(int x, int y){return damageGrid[x][y];}

	public int takeDamage(int x, int y, int health){
		if(health<damageGrid[x][y])
		{
			damageGrid[x][y] -= health;
			return health;
		}
		else
		{
			damageGrid[x][y] -= health;
			return damageGrid[x][y];
		}
	}


	public void turn(double decay){
		for(int i=0; i<64; i++)
			for(int j=0; j<64; j++)
			{
				enemyFeromones[i][j] *= decay;
				for(int k=0; k<4; k++)
					activeFeromones[i][j][k] *= decay;
			}
	}
}




