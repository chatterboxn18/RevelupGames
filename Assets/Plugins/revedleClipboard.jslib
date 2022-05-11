mergeInto(LibraryManager.library, {

  CopyToClipboard: function (month, day, array, guesses) {
	  var miss = "⬛";
	  var almost = "🟥";
	  var correct = "💗";

	  var returnString = month + "/" + day + " Revedle " + Pointer_stringify(guesses) + "/6\n" ;
	  console.log(Pointer_stringify(array));
	  returnString += Pointer_stringify(array);
	  returnString += "\nhttps://thelamgoesmoo.github.io/revedle #RedVelvet #revedle";
	  navigator.clipboard.writeText(returnString);
  },

  OpenLink: function (link) {
	window.open(Pointer_stringify(link), '_blank');
  }

});