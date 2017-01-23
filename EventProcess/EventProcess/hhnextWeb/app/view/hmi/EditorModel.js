Ext.define('MyApp2.view.hmi.EditorModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.hmieditor',
    data: {
        name: 'MyApp2',
		// polyonSideCount:5,
		// polyonSpikeCount:5,
		toolbar:{},
		property:{
			// strokeWidth:1,
			// backgroundColor:'green',
			// fill:'green'
		},
		propertyKey:true
		
    }
	// ,
	// formulas: {
       // lineWidth: {
			// bind: {
                // x: {
					// bindTo: '{property}'
					
					// }
               
            // },
			// get: function (get) {
				// return this.get('x.strokeWidth');
			// }
			// // ,
			
			// // set: function (value) {
				// // data.x=value;
			// // }
				
		// }
	// }
	
	
});
