//import java.awt.BasicStroke;
import java.awt.Color;
//import java.awt.Font;
//import java.awt.FontMetrics;
//import java.awt.GradientPaint;
import java.awt.Graphics2D;
//import java.awt.geom.Ellipse2D;
import java.awt.image.BufferedImage;
import java.awt.geom.Rectangle2D;
import javax.imageio.stream.*;
import java.io.File;
import java.io.IOException;
//import javax.imageio.ImageIO;

public class antColony {
  static public void main(String args[]) throws Exception {
     try {
      int width = 64, height = 64, multiplier = 16;
      int[][][] picture = new int[width][height][3];

      // TYPE_INT_ARGB specifies the image format: 8-bit RGBA packed
      // into integer pixels
      BufferedImage bi = new BufferedImage(width*multiplier, height*multiplier, BufferedImage.TYPE_INT_ARGB);
      ImageOutputStream output = new FileImageOutputStream(new File("test.gif"));
      Graphics2D ig2 = bi.createGraphics();
      GifSequenceWriter writer = new GifSequenceWriter(output, bi.getType(), 1, false);
      //writer.writeToSequence(bi);

      // initializing main Loop
      Referee referee = new Referee(100, width, height);


      Player player1 = new Player(new int[]{0,255,0}, 0, 24, 32, 50, referee, new KeKroepes_Q());
      Player player2 = new Player(new int[]{70,0,255}, 1, 40, 32, 50, referee, new KeKroepes_Q());
      //player1.draw(picture);

      // main loop
      for(int frame = 0; frame<2500; frame++){
        // clear image
        ig2.setPaint(new Color(15,15,15));
        ig2.fill(new Rectangle2D.Double(0, 0, width*multiplier, height*multiplier));
        
        // execute player turns

        player1.turn();
        player2.turn();
        referee.turn();

        if(frame%100==0){
          System.out.printf("frame:%d\n", frame);
        }

        // only draw a few frames
        if(frame%10==0){
        //draw everything
        for(int i=0; i<width; i++)
          for(int j=0; j<height; j++)
            for(int k=0; k<3; k++)
              picture[i][j][k] = 15;

        referee.draw(picture);
        player1.draw(picture);  
        player2.draw(picture);  

        for(int i=0; i<width; i++)
          for(int j=0; j<height; j++)
            for(int k=0; k<3; k++)
              if(picture[i][j][k]>255)
                picture[i][j][k] = 255;

        for(int i=0; i<width; i++)
          for(int j=0; j<height; j++)
            {
              if(picture[i][j][0]!=15 || picture[i][j][1]!=15 || picture[i][j][2]!=15){
                ig2.setPaint(new Color(picture[i][j][0],picture[i][j][1],picture[i][j][2]));
                ig2.fill(new Rectangle2D.Double(i*multiplier, j*multiplier, multiplier, multiplier));
              }
            }
        ig2.setPaint(Color.white);
        ig2.drawString(String.format("Fr: %d", frame),10,10);

        ig2.setPaint(new Color(player1.getColor()[0],player1.getColor()[1],player1.getColor()[2]));
        ig2.drawString(String.format("P1: %d", player1.colonySize()),10,25);

        ig2.setPaint(new Color(player2.getColor()[0],player2.getColor()[1],player2.getColor()[2]));
        ig2.drawString(String.format("P2: %d", player2.colonySize()),10,40);
        

        //store the image into the gif
        writer.writeToSequence(bi);
        }
      }

      System.out.printf("player 1: %d ants\n",player1.colonySize());
      System.out.printf("player 2: %d ants\n",player2.colonySize());

      // close the gif output
      writer.close();
      output.close();
    } catch (IOException ie) {
      ie.printStackTrace();
    }
}
}