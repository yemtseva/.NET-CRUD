#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>

struct Movie {
   int year;
   char languages[5][20];
   char title[50];
   double rating;
   char notSlicedLanguages[100];
};

struct Node {
   struct Movie data;
   struct Node* next;
};


void sliceLanguages(struct Node* currentNode){
 
   while(currentNode->next){
      int i = 0;
      char* token = strtok(currentNode->data.notSlicedLanguages, ";");
      while(token){
         strcpy(currentNode->data.languages[i], token);
         token = strtok(NULL, ";");
         i++;
      }
      char* token2 = strtok(currentNode->data.languages[0], "[");
      char* token3 = strtok(currentNode->data.languages[i-1], "]");
      strcpy(currentNode->data.languages[0], token2);
      strcpy(currentNode->data.languages[i-1], token3);
      currentNode = currentNode->next;
   } 
   

}

struct Node* read_csv(char *filename, int *movie_num){
   FILE *file;
   file = fopen(filename, "r");
   if(file == NULL){
      perror("Failed: ");
      exit(1);
   }

   struct Node* head = (struct Node*) malloc(sizeof(struct Node));
   struct Node* currentNode = (struct Node*) malloc(sizeof(struct Node));
   head->next = currentNode;
   char buff[1024];
   int row_count = 0;
   int field_count = 0;

   while(fgets(buff, 1024, file)){
      row_count++;
      (*movie_num)++;
      field_count = 0;
      if(row_count == 1)
	 continue;

      char *field = strtok(buff, ",");
      
      
      while(field_count < 4 ){
	
	
	 if(field_count == 0){
	    strcpy(currentNode->data.title, field);

	 }

	 if(field_count == 1){
	    currentNode->data.year = atoi(field);
	   
	 }
	 if(field_count == 2){
	    strcpy(currentNode->data.notSlicedLanguages, field);
	 }
	 
	 if(field_count == 3){
	    currentNode->data.rating = strtod(field, NULL);

	 }
	 field = strtok(NULL, ",");
	 field_count++;
	 
      }
      struct Node* newNode = (struct Node*) malloc(sizeof(struct Node));
      currentNode->next = newNode;
      currentNode = currentNode->next;
   
   }
   fclose(file);
   (*movie_num)--;
   return head;

}

void moviesInYear(struct Node* currentNode, int year){
   bool isExist = false;
   while(currentNode->next){
      if(year == currentNode->data.year){
	 printf("%s\n", currentNode->data.title);
	 isExist = true;
      }
      currentNode = currentNode->next;
   
   }
   if(isExist == false)
      printf("No data about movies released in the year %d\n", year);


}

int getYears(struct Node* currentNode, int *years ){
   int i = 0;
   int q;
   int years_num = 1;
   bool isExist = false;
   while(currentNode->next){
      isExist = false;
      for(q = 0; q < i; q++){
	 if(years[q] == currentNode->data.year){
	    isExist = true;
	    years_num++;
	    break;
	 }
      }
      
      if(isExist == false){
	 years[i] = currentNode->data.year;
	 i++;
      }
      currentNode = currentNode->next;
      
   }
   return years_num;


}

void highestRatingInYears(struct Node* head, int movie_num){

   struct Node* currentNode = head->next;
   struct Node* resultNode = NULL;
   double resultRating = 0;
   int years[movie_num];
   int unique_years;
   unique_years = getYears(head->next, years);
 
   int i;
   for(i = 0; i < unique_years; i++){
      currentNode = head->next;
      while(currentNode->next){
	 if(currentNode->data.year == years[i] && currentNode->data.rating > resultRating){
	    resultRating = currentNode->data.rating;
	    resultNode = currentNode;
	 }
	 currentNode = currentNode->next;
      }
    
      printf("%d %f %s\n", resultNode->data.year, resultNode->data.rating, resultNode->data.title);
      resultNode = NULL;
      resultRating = 0;
   }
   
}

void moviesInLanguage(struct Node* currentNode, char* language){
   bool isExist = false;
   while(currentNode->next){
      int i;
      for(i = 0; i < 5; i++){
	
	 if(strcmp(language, currentNode->data.languages[i]) == 0){
	    printf("%d %s\n", currentNode->data.year, currentNode->data.title);
	    isExist = true;
	 }
	
      }
      currentNode = currentNode->next;
   }
   if(isExist == false)
      printf("No data about movies released in %s\n", language);

}

int main(int argc, char *argv[]){

   struct Movie m;
   struct Node* head = NULL;

   int year;
   int choice;
   int movie_num = 0;
   char language[20];
   //int arrYears[12];

   head = read_csv(argv[1], &movie_num);
   printf("Processed file %s and parsed data for %d movies\n", argv[1], movie_num);

   do{
   printf("1. Show movies released in the specified year\n");
   printf("2. Show highest rated movie for each year\n");
   printf("3. Show the title and year of release of all movies in a specific language\n");
   printf("4. Exit from the program\n");
   scanf("%d", &choice);

   if(choice == 1){
     printf("Enter the year for which you want to see movies: \n");
     scanf("%d", &year);
     moviesInYear(head->next, year);
   }
   else if(choice == 2){
      highestRatingInYears(head->next, movie_num);
   }
   else if(choice == 3){
      printf("Enter the language for which you want to see movies: \n");
      scanf("%s", &language);
      sliceLanguages(head->next);
      moviesInLanguage(head->next, language);
   }
   else if(choice == 4){
      exit(1);
   }
   else{
      printf("You entered an incorrect choice. Try again");
   }
   }while(choice != 4);


   return 0;
}
