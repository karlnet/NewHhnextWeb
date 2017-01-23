
Ext.define('MyApp2.view.hmi.Editor',{
    extend: 'Ext.container.Container',
	// title:'HMI Editor',
	width: 1050,
    height: 800,
	xtype: 'hmi-editor',
	
	header: {
        layout: {
            align: 'stretchmax'
        },
        title: 'hmi',      
        iconCls: 'fa-th-list'
    },
	
    requires: [
        'MyApp2.view.hmi.EditorController',
        'MyApp2.view.hmi.EditorModel',
		'MyApp2.view.hmi.MyCanvans',
		'Ext.ux.colorpick.Field',
		'Ext.ux.colorpick.Button'
    ],
	
    controller: 'hmieditor',
    viewModel: {
        type: 'hmieditor'
    },
	
	layout: {
        type: 'border',
		regionWeights: {
         west: 10,
         north: 20,
         south: 20,
         east: 10
     }
    },
	listeners: {
			afterrender: 'onAfterrender'
		   
	},
    items: [{
    	xtype : 'toolbar',
		id:'toolbar',
    	region: 'north',
		// height:50,
		split: true,
		// layout: 'form',
		items:[{
			xtype: 'component',
			// margin:'0 0 0 0',
			id: 'my-toolbar-size',	
			html:'长X宽 :'
		},{	
			xtype:'textfield',
			// fieldLabel:'画面尺寸',
			layout: 'anchor',
			name:'canvassize',
			emptyText:'1800*1600',
			width:90,
			
		},{
			xtype: 'segmentedbutton',
            items: [{
				// text:'组合',
				handler: 'onGroup',
				iconCls: 'my-toolbar-group'
			},
			{
				// text:'分解',
				handler: 'onUnGroup',
				iconCls: 'my-toolbar-ungroup'
			},
			{
				// text:'向上一层',
				handler: 'onBringForward',
				iconCls: 'my-toolbar-top'
				
			},{
				// text:'到顶层',
				handler: 'onBringToFront',
				iconCls: 'my-toolbar-toptop'
				
			},{
				// text:'向下一层',
				handler: 'onSendBackwards',
				iconCls: 'my-toolbar-back'
			},{
				// text:'到底层',
				handler: 'onSendToBack',
				iconCls: 'my-toolbar-backback'
			
			},
			{
				// text:'左旋90度',
				handler: 'onRotateLeft',
				iconCls:'my-toolbar-rotateLeft'
				// glyph:0xf0e2
			},
			{
				// text:'右旋90度',
				handler: 'onRotateRight',
				iconCls: 'my-toolbar-rotateRight'
				// glyph:0xf01e
			},
			{
				handler: 'onRotateRight',
				iconCls: 'my-toolbar-alignleft'
			},
			{
				handler: 'onRotateLeft',
				iconCls:'my-toolbar-alignright'
			},
			{
				handler: 'onRotateRight',
				iconCls: 'my-toolbar-alignbottom'
			},
			{
				handler: 'onRotateLeft',
				iconCls:'my-toolbar-aligntop'
			},
			{
				handler: 'onRotateLeft',
				iconCls:'my-toolbar-aligncenter'
			},{
				handler: 'onRotateRight',
				iconCls: 'my-toolbar-aligncenter2'
		}]},{
			xtype: 'component',
			bind:{
				hidden:'{!toolbar.fill}'
			},
			cls: 'my-toolbar-fill',	
			html:'填'
		},{	
			xtype: 'colorbutton',		
			bind: {
				hidden:'{!toolbar.fill}',
				value: {
					bindTo: '{property.fill}',
					deep: true
				}
			},
			// width: 15,
			// height: 15,
			listeners: {
				change: 'onSetFillColor',
				focus :'onFillFocus',
				statesave:'onFillStateSave'
			}
		},{
			xtype: 'component',
			bind:{
				hidden:'{!toolbar.stroke}'
			},
			cls: 'my-toolbar-fill',	
			html:'边'
			
		},{	
			xtype: 'colorbutton',		
				bind: {
				hidden:'{!toolbar.stroke}',
				value: {
					bindTo: '{property.stroke}',
					deep: true
				}
			},
			listeners: {
				change: 'onSetStrokeColor',
			}
		},{
			xtype: 'container',
			// text:'线宽',
			layout: {
				type: 'hbox',
				align: 'center'
			},
			items: [{
				xtype: 'slider',
				
				reference:'strokeWidthslider',
				increment: 1,
				minValue: 0,
				maxValue: 30,
				width: 80,
				hasFocus:true,
				publishOnComplete: false,
				margin: '0 0 0 8',
				bind: {
					hidden:'{!toolbar.strokeWidth}',
					value:'{property.strokeWidth}'		
				},
				listeners:{
					change:'onStrokeWidthChange'
				}
			},{
				xtype: 'component',
				margin:'0 0 0 8',
				id: 'my-toolbar-strokeWidth',	
				bind:{
					hidden:'{!toolbar.strokeWidth}',
					html: '{property.strokeWidth}'
				}
			}]},
			,
			{
				xtype: 'component',
				bind:{
					hidden:'{!toolbar.sideCount}'
				},
				cls: 'my-toolbar-fill',	
				html:'边'
			},
			{
				xtype:'numberfield',
				// fieldLabel:'边数',
				name:'polygonSideCount',
				reference:'polygonSideCount',
				bind:{
					hidden:'{!toolbar.sideCount}',
					value:'{property.sideCount}'	
				},
				width:70,
				listeners:{
					change:'onSideCountChange'
				}
				
			},
			{
				xtype: 'component',
				bind:{
					hidden:'{!toolbar.spikeCount}'
				},
				cls: 'my-toolbar-fill',	
				html:'角'
			},
			{
				xtype:'numberfield',
				name:'polygonSpikeCount',
				reference:'polygonSpikeCount',
				bind:{
					hidden:'{!toolbar.spikeCount}',
					value:'{property.spikeCount}'	
				},
				width:70,
				listeners:{
					change:'onSpikeCountChange'
				}
			},
			'->'
			,{
				iconCls: null,
				glyph: 0xf04b,
				text:'Run',
				handler:'onRun'
			}
			,{
				iconCls: null,
				glyph: 0xf0c7,
				text:'Save',
				handler:'onSaveCanvas'
			}
		]
	},
	{
        xtype: 'panel',
        region: 'west',        
        width: 130,
        split: true,
		collapsible:true,
		title:'工具箱',
		default:{
			bodyStyle:'padding；5px'
		},
		layout:{
			type:'accordion',
			activeOnTop:true,
			titleCollapse:false
		},
		items:[{
			title:'常用工具',
			layout:{
				type:'auto'
			},
			
			defaultType: 'button',
			defaults:{
				width:60
			},
			items:[{
				
				text:'pointer',
				handler: 'onPointerToolSelected'
			},
			{				
				text:'rect',
				handler: 'onRectToolSelected'
			},
			{
				text:'circle',
				handler: 'onCircleToolSelected'
			},
			{
				text:'text',
				handler: 'onTextToolSelected'
			},
			{
				text:'triangle',
				handler: 'onTriangleToolSelected'
			},
			{
				text:'ellipse',
				handler: 'onEllipseToolSelected'
			},
			{
				text:'line',
				handler: 'onlineToolSelected'
			},
			{
				text:'polygon',
				handler: 'onPolygonToolSelected'
			},{
				text:'polyline',
				handler: 'onPolylineToolSelected'
			},{
				text:'hexagon',
				handler: 'onHexagonToolSelected'
			},{
				text:'starPolygon',
				handler: 'onStarPolygonToolSelected'
			},{
				text:'thermometer',
				handler: 'onThermometerToolSelected'
			},{
				text:'tank',
				handler: 'onTankToolSelected'
			},{
				text:'lamp',
				handler: 'onLampToolSelected'
			},{
				text:'switch',
				handler: 'onSwitchToolSelected'
			},{
				text:'Motor',
				handler: 'onMotorToolSelected'
			},{
				text:'Fan',
				handler: 'onFanToolSelected'
			},{
				text:'Pipe',
				handler: 'onPipeToolSelected'
			},{
				text:'Valve',
				handler: 'onValveToolSelected'
			},{
				text:'Dashboard',
				handler: 'onDashboardToolSelected'
			}
		]
			
			
			
		},{
			title:'常用控件'
		}]
		
    },{
        xtype: 'hmi-canvans',
		region: 'center',
		flex:1
		
    },{
        xtype: 'propertygrid',
		region: 'east',
		width: 220,
		split: true,
		collapsible:true,
		id:'mypropertygrid',
		reference:'mypropertygrid',
		padding:'0 5 0 5',
		title: '属性面板',
		listeners:{				
			'propertychange':'onPropertychange'
				
		},
		viewConfig: {
			forceFit: true,
			scrollOffset: 2 // the grid will never have scrollbars
		},
		bind: {
			source: {
				bindTo: '{property}',
				deep: true
			}
			
		} ,
		sourceConfig: {
			// clientIsAvailable: {
				// // Custom renderer to change the color based on the value
				// renderer: function(v){
					// var color = v ? 'green' : 'red';
					// return '<span style="color: ' + color + ';">' + v + '</span>';
				// }			
			// },
			
			width:{
				 // displayName: '宽度'
				 renderer: function(v){
					return '<span  style=" color: ' + v + ';">' + v + '</span><button onClick="cellItem=\'width\'" class="my-property-fill" />'
				 }
			},
			height:{
				 // displayName: '高度'
				 renderer: function(v){
					return '<span  style=" color: ' + v + ';">' + v + '</span><button onClick="cellItem=\'height\'" class="my-property-fill" />'
				 }
			},
			left:{
				 // displayName: 'X坐标'
				
			},
			top:{
				 // displayName: 'Y坐标'
				
			},
			angle:{
				 // displayName: '角度'
				renderer: function(v){
					return '<span  style=" color: ' + v + ';">' + v + '</span><button onClick="cellItem=\'angle\'" class="my-property-fill" />'
				 }
			},
			fill:{
				 // displayName: '填充色',
				 renderer: function(v){
					return '<span  style=" color: ' + v + ';">' + v + '</span><button onClick="cellItem=\'fill\'" class="my-property-fill" />'
				 },
				 editor: {
					xtype: 'colorfield',
					// listeners: {
					   // change: function (picker) {
						   // // metaData.up('grid').getSelection()[0].dirty = true;
						   // // metaData.up('grid').getSelectionModel().getSelection()[0].data.colorCode = value;
					   // }
					// }
				 }
				
			},
			text:{
				 displayName: '文本',
				 renderer: function(v){
					return '<span >' + v + '</span><button  onClick="cellItem=\'text\'" class="my-property-fill" />';
				 }
				
			},
			opacity:{
				 // displayName: '透明度'
				renderer: function(v){
					return '<span >' + v + '</span><button  class="my-property-fill" />'
				 },
			},
			stroke:{
				 // displayName: '边线颜色'
				
			},
			strokeWidth:{
				 // displayName: '边线宽度'
				renderer: function(v){
					return '<span >' + v + '</span><button  onClick="cellItem=\'strokeWidth\'" class="my-property-fill" />'
				 },
			},
			strokeLineCap:{
				 // displayName: '边线颜色',
				editor: {
					xtype: 'combobox',
					store: ['butt', 'round', 'square'],
					forceSelection: true,
					allowBlank: false
				}
				
			},
			strokeLineJoin :{
				 // // displayName: '边线颜色',
				editor: {
					xtype: 'combobox',
					store: ['bevil', 'round', 'miter'],
					forceSelection: true,
					allowBlank: false
				}
				
			},
			
			myValue:{
				renderer: function(v){
					return '<span >' + v + '</span><button  onClick="cellItem=\'myValue\'" class="my-property-fill" />'
				 }
			},
			backgroundColor:{
				 // displayName: '背景色'
				
			} 
			
		}
		
		
		
    }]
	
	
});
