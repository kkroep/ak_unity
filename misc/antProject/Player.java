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
	private ProtectedPlayer protectedPlayer = new ProtectedPlayer(this);
	private QueenBrain queenBrain = new QueenBrain();
	private Referee referee;


	public Player(int[] color, int playerNumber, int x, int y, int food, Referee referee, QueenBrain queenBrain) {
		this.x = x;
		this.y = y;
		this.color = color;
		this.food = food;
		this.referee = referee;
		this.playerNumber = playerNumber;
		this.ants = new ArrayList<Ant>();
		this.queenBrain = queenBrain;
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
			if(getPlayerNumber()==0)
				ants.add(new Ant(x,y, referee, new KeKroepes(), playerNumber, 3, 1, 1, 100)); 
			else
				ants.add(new Ant(x,y, referee, new KeKroepes(), playerNumber, 3, 1, 1, 100));
			food -=5;
			return true;
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


class ProtectedPlayer{
	private Player player;

	public ProtectedPlayer(Player player){
		this.player = player;
	}

	public int colonySize(){return player.colonySize();}
	
	// returns true if it is succesfull and false if not
	public boolean createAnt(){return player.createAnt();}

	public int getFood(){return player.getFood();}

}

// class that can be extended to form the queen brain
class QueenBrain{
	public QueenBrain(){}

	// called every an ant returns to the queen location
	public void reportingAnt(AntProperties antProperties){
		//System.out.printf("%d", antProperties.ge)
	}

	// called at the start of every turn
	public void turn(ProtectedPlayer player){
		/*if(player.getFood()>=5){
			player.createAnt();
		}*/

	} 
}




