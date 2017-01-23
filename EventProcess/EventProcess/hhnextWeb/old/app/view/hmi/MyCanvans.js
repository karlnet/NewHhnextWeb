
Ext.define('MyApp2.view.hmi.MyCanvans',{
    extend: 'Ext.panel.Panel',
	xtype: 'hmi-canvans',
    requires: [
        'MyApp2.view.hmi.MyCanvansController',
        'MyApp2.view.hmi.MyCanvansModel'
    ],

    controller: 'hmieditor',
    viewModel: {
        type: 'mycanvans'
    },
	// layout:'fit',
	// listeners: {
        // afterrender: 'onAfterrender'
       
    // },
	// width: 800,
	// height:800,
    // bodyPadding: 10,
	items: [{
            scroll: false,
            html: "<canvas width='800' id='canvas' height='500'/>"
        },{
			xtype: 'panel',
			items:[{
					xtype: 'textfield',
					name: 'textfield1',
					fieldLabel: 'Text field',
					reference:'textfield1',
					value: "'$0:1'"
				},{
					xtype: 'textareafield',
					name: 'textarea1',
					fieldLabel: 'TextArea',
					reference:'textarea1',
					value: "function() {  if ($0:1%2===0) return 'blue';else return 'red' ; }"
				
				}
			]
		}, {
			xtype: 'toolbar',
			items:[{
					xtype: 'button',			
					html: 'editor',
					handler:'onScript'
					
				},
				'-',
				{
					xtype: 'button',			
					html: 'save',
					handler:'onSaveCanvas'
				
				},{
					xtype: 'button',			
					html: 'run',
					handler:'onRun'
				
				}
			]
		}]
		

   
});
