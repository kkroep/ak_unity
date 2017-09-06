import java.awt.Graphics2D;
import java.awt.Color;
import java.awt.geom.Rectangle2D;
import java.util.*;
import java.lang.reflect.Constructor;


class Player{
	private int x;
	private int y;
	private int food;
	private int[] color;
	private int playerNumber;
	private ArrayList<Ant> ants;
	private ProtectedPlayer protectedPlayer = new ProtectedPlayer(this);
	private QueenBrain queenBrain = new QueenBrain();
	private Referee referee;
	private Constructor antBrainCtor;

	public Player(int[] color, int playerNumber, int x, int y, int food, Referee referee, QueenBrain queenBrain, Constructor antBrainCtor) {
		this.x = x;
		this.y = y;
		this.color = color;
		this.food = food;
		this.referee = referee;
		this.playerNumber = playerNumber;
		this.ants = new ArrayList<Ant>();
		this.queenBrain = queenBrain;
		this.antBrainCtor = antBrainCtor;
	}

	// set functions
	//public void setX(int x){this.x = x;}
	//public void setY(int y){this.y = y;}
	//public void setPlayerNumber(int playerNumber){this.playerNumber = playerNumber;}

	// get functions
	public int getX(){return x;}
	public int getY(){return y;}
	public int[] getColor(){return color;}
	public int getFood(){return food;}
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

	public void reportingAnt(AntProperties antProperties){
		queenBrain.reportingAnt(antProperties);
	}

	public boolean createAnt(){
		if(food>=5){
			try
			{
				ants.add(new Ant(x,y, referee, (AntBrain)antBrainCtor.newInstance(), playerNumber, 3, 1, 1, 100)); 
				food -=5;
				return true;
			}
			catch(Exception e){
				System.out.printf("EXCEPTION: failed to make ant\n");
				return false;
			}	
		}
		else
			return false;
	}

	// execute a turn
	public final void turn(){
		queenBrain.turn(protectedPlayer);


		// turn for the ants
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


