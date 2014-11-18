﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;

namespace RT
{
	public partial class RTMovieView : DialogViewController
	{
		public MovieRootObject movieDetails { get; set; }
		public ReviewRootObject reviewList { get; set; }
		public RTRepository repository { get; set; }
		string title = null; 
		UINavigationController navControl;
		public RTMovieView (MovieRootObject r, ReviewRootObject q, UINavigationController navControl) : base (UITableViewStyle.Grouped, null)
		{
			movieDetails = r;
			reviewList = q; 
			this.navControl = navControl;
			title = r.title; 
		}

		public RootElement getUI(){
			var RootElement = new RootElement (title);

			if (movieDetails.abridged_cast.Count > 0) {
				var Cast = new Section ("Cast"); 
				foreach (var actor in movieDetails.abridged_cast) {
					var a = new StringElement (actor.name);
					Cast.Add (a);
				}
				RootElement.Add (Cast);
			}

			if (movieDetails.abridged_directors.Count > 0) {
				var DirectedBy = new Section ("Directed By"); 
				var directorString = "";
				for (int i = 0; i < movieDetails.abridged_directors.Count; i++)
					directorString += (movieDetails.abridged_directors.Count - 1 == i) ? movieDetails.abridged_directors [i].name : movieDetails.abridged_directors [i].name + ", ";
				var directors = new StringElement (directorString);
				DirectedBy.Add (directors);
				RootElement.Add (DirectedBy);
			}

			if (movieDetails.mpaa_rating != null) {
				var MPAARating = new Section ("MPAA Rating"); 
				var mpaaRating = new StringElement (movieDetails.mpaa_rating);
				MPAARating.Add (mpaaRating);
				RootElement.Add (MPAARating);
			}

			if (movieDetails.runtime.HasValue) {
				var movieRuntime = new Section ("Runtime");
				var runtime = new StringElement (string.Format ("{0} hr. {1} min.", movieDetails.runtime / 60, movieDetails.runtime % 60));
				movieRuntime.Add (runtime);
				RootElement.Add (movieRuntime);
			}

			if (movieDetails.genres.Count > 0) {
				var Genres = new Section ("Genre(s)"); 
				foreach (var genre in movieDetails.genres) {
					var g = new StringElement (genre);
					Genres.Add (g);
				}
				RootElement.Add (Genres);
			}
				
			if (movieDetails.release_dates.theater != null) {
				var ReleaseDate = new Section ("Release Date"); 
				var date = new StringElement (movieDetails.release_dates.theater);
				ReleaseDate.Add (date);
				RootElement.Add (ReleaseDate);
			}

			if (movieDetails.synopsis != null) {
				var Synopsis = new Section ("Synopsis");
				var synopsisText = new MultilineElement (movieDetails.synopsis);
				Synopsis.Add (synopsisText);
				RootElement.Add (Synopsis);
			}

			if (reviewList.reviews.Count > 0) {
				var CriticReviews = new Section ("Critic Reviews");
				foreach (Review R in reviewList.reviews) {
					var reviewEl = new ReviewElement (R);
					CriticReviews.Add (reviewEl);
				}
				RootElement.Add (CriticReviews);
			}

			return RootElement;
		}
	}

}
