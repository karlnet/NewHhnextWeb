
var theCanvas;
var actionElement='idel';
var cellItem='';

		
Ext.define('MyApp2.view.hmi.EditorController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.hmieditor',
    
	requires: [
        'MyApp2.view.hmi.MyCanvans',
		'Ext.util.DelayedTask'
    ],
	
	init: function() {
        
    },
	
	control:{
             'propertygrid field': {
                 change: function(field, newValue, oldValue, eOpts){
						if(typeof theCanvas!='undefined'&&newValue!='null'){
							// vm.set('property',vm.get('property'));
							var propertyName= field.previousNode().editorId;
							
							if(propertyName==='fill'){
								newValue='#'+newValue;
								
							}
							
							this.updateCavansObject(propertyName,newValue);
						}
                 }
             },
			 
			 'propertygrid':{
				'cellclick':function( td , cellIndex , record , tr , rowIndex , e , eOpts ) {
					var activeObject = theCanvas.getActiveObject();
					if(cellItem!=''&&activeObject){
						
						Ext.MessageBox.show({
							title: 'Script:',
							msg: 'Please enter your script for : <span style="margin:10;color:#FF0000;">'+cellItem+'</span>',
							width:400,
							defaultTextHeight :200,
							buttons: Ext.MessageBox.OKCANCEL,
							multiline: true,
							// scope: this,
							// icon: Ext.MessageBox.INFO,
							buttonText :{
								ok:'Save'
							},
							value :(function(){
								var activeObject = theCanvas.getActiveObject();
								if(activeObject){
									if(activeObject.shapeScript===null){
										activeObject.shapeScript={};
									}
									return activeObject.shapeScript[cellItem]||'';
									
								}
							})(),
							fn: function(buttonId ,text ,opt ){
								if(buttonId==='ok'){
									var activeObject = theCanvas.getActiveObject();
									if(activeObject){
										// console.log(activeObject.shapeScript);
										// activeObject.shapeScript=activeObject.shapeScript||{};
										// console.log(activeObject.shapeScript);
										activeObject.shapeScript[cellItem]=text;
									}
									console.log(activeObject.shapeScript);
								}
								cellItem='';
							}
						});
						// console.log(td, 'cellIndex = '+cellIndex ,' record  = '+record, tr , rowIndex , e ,cellItem);
					
						
					}
					
				}
			 }
    },
	//theCanvas : undefined,
	
	// constructor: function(config) {
		// this.callParent(arguments);
		
		// //new fabric.Canvas('canvas');
		
	 // },
	
	onAfterrender:function() {
		
		// Object.prototype.isEmptyObject= function () {
			// var prop;
			// for ( prop in this)
				// return false;
			// return true;
		// };
		
		
		// theCanvas=null;
		
		Ext.get('toolbar').on('mouseover',function(){
			actionElement='toolbar';
			// console.log('mouse down');
		});
		// Ext.get('toolbar').on('mouseout',function(){
			// actionElement='idel';
			// console.log('mouse up');
		// });
		var me=this;
		var mvvm=me.getViewModel();
		
		var needTools={stroke:false,strokeWidth:false,fill:false,sideCount:false,spikeCount:false};
		mvvm.set('toolbar',needTools)
		
		var regularPolygonPoints=function (sideCount,radius){
			var sweep=Math.PI*2/sideCount;
			var cx=radius;
			var cy=radius;
			var points=[];
			for(var i=0;i<sideCount;i++){
				var x=cx+radius*Math.cos(i*sweep);
				var y=cy+radius*Math.sin(i*sweep);
				points.push({x:x,y:y});
			}
			return (points);
		};
		var starPolygonPoints= function (spikeCount, outerRadius, innerRadius) {
			var rot = Math.PI / 2 * 3;
			var cx = outerRadius;
			var cy = outerRadius;
			var sweep = Math.PI / spikeCount;
			var points = [];
			var angle = 0;

			for (var i = 0; i < spikeCount; i++) {
				var x = cx + Math.cos(angle) * outerRadius;
				var y = cy + Math.sin(angle) * outerRadius;
				points.push({x: x, y: y});
				angle += sweep;

				x = cx + Math.cos(angle) * innerRadius;
				y = cy + Math.sin(angle) * innerRadius;
				points.push({x: x, y: y});
				angle += sweep
			}
			return (points);
		};
		
		if (typeof theCanvas==='undefined'){
			theCanvas= new fabric.Canvas('canvas');
			
			theCanvas.backgroundColor = 'rgba(150, 150, 150, 0.1)';
			theCanvas.activeTool='Pointer';
			theCanvas.isActiveObjectGroup='blank';  //  blank ,object , groupObject , activeGroup
			theCanvas.strokeWidth=2;
			theCanvas.stroke='#FFCC00';
			theCanvas.fill='#33CCCC';
			theCanvas.sideCount=5;
			theCanvas.spikeCount=5;
			theCanvas.type='canvas';
				
			
			// var canvasWidth = theCanvas.getWidth();
			// var canvasHeight = theCanvas.getHeight();
			
			// var grid = 1;
			// var i=0;
			// while((i*grid)<=canvasHeight){
				// theCanvas.add(new fabric.Line([ 0,i * grid,canvasWidth,i * grid ], { stroke: '#ccc', selectable: false }));
				// i++;
			// }
			// var j=0;
			// while((j*grid)<=canvasWidth){
				// theCanvas.add(new fabric.Line([ j * grid,0, j * grid, canvasHeight], { stroke: '#ccc', selectable: false }));
				// j++;
			// }
		}
		

		// fabric.Canvas.prototype.leftButtonPressed=false;
		
		fabric.Object.prototype.transparentCorners = false;
		// fabric.Object.prototype.borderColor = 'red';
		fabric.Object.prototype.cornerColor = '#006600';
		fabric.Object.prototype.cornerSize = 8;
		
		
		fabric.Object.prototype.zIndex=0;
		fabric.Object.prototype.getZIndex = function() {
			return theCanvas.getObjects().indexOf(this);
		};
		fabric.Object.prototype.shapeScript=null;
		
		// var el = Ext.get('canvas');
		// console.log(el);
		fabric.util.addListener(window, 'keyup', function (e) {
			// console.log(e.key);
			if (e.key === 'Delete') {
				
				me.removeCavansObject();
				
			}
		});
		
		ObjectToolBar=['fill','stroke','strokeWidth'];
		RectToolBar=[];
	    CircleToolBar=[];		
		EllipseToolBar=[];
		TriangleToolBar=[];
		LineToolBar=['strokeLineCap'];
		TextToolBar=[];
		PolygonToolBar=[];
		
		fabric.Object.prototype.baseToolBar=ObjectToolBar;
		fabric.Rect.prototype.toolBar=RectToolBar;
		fabric.Circle.prototype.toolBar=CircleToolBar;
		fabric.Ellipse.prototype.toolBar=EllipseToolBar;
		fabric.Triangle.prototype.toolBar=TriangleToolBar;
		fabric.Line.prototype.toolBar=LineToolBar;
		fabric.Text.prototype.toolBar=TextToolBar;	
		fabric.Polygon.prototype.toolBar=PolygonToolBar;	
		
		
		CanvasProperty=['type','width','height','fill','stroke','strokeWidth','sideCount','spikeCount'];
		ObjectProperty=['zIndex','left','top','width','height','angle','fill','opacity','type','scaleX','scaleY','stroke','strokeWidth'];
		RectProperty=[];
	    CircleProperty=['radius','startAngle','endAngle'];		
		EllipseProperty=['rx','ry'];
		TriangleProperty=[];
		LineProperty=['strokeLineCap'];
		TextProperty=['fontFamily','fontSize','fontWeight','textAlign','fontStyle','text','textBackgroundColor'];
		PolygonProperty=[];

		fabric.Canvas.prototype.property=CanvasProperty;		
		fabric.Object.prototype.baseProperty=ObjectProperty;
		fabric.Rect.prototype.property=RectProperty;
		fabric.Circle.prototype.property=CircleProperty;
		fabric.Ellipse.prototype.property=EllipseProperty;
		fabric.Triangle.prototype.property=TriangleProperty;
		fabric.Line.prototype.property=LineProperty;
		fabric.Text.prototype.property=TextProperty;	
		fabric.Polygon.prototype.property=PolygonProperty;	
		
		fabric.Object.prototype.MoveHandleTo= function(pointer) {
			this.set({
				width:pointer.x-this.getLeft(),
				height:pointer.y-this.getTop()
			});
		}
		fabric.Circle.prototype.MoveHandleTo= function(pointer) {
			var width=pointer.x-this.getLeft(),
				height=pointer.y-this.getTop();
				
			this.set({
				radius:Math.min(width, height)/2				
			});
		}
		fabric.Ellipse.prototype.MoveHandleTo= function(pointer) {
			var width=pointer.x-this.getLeft(),
				height=pointer.y-this.getTop();
				
			this.set({
				rx:width/2,ry:height/2				
			});
		}
		
		
		
		// var grid=this.lookupReference('mypropertygrid');
		
		var onSelectProperty = function (options) {
			
			var vm=me.getViewModel();
			var shape=options.target;	
			var key='';
			var baseProperty=shape.baseProperty||[];
			var baseToolBar=shape.baseToolBar||[];
			var shapeToolBar={};
			
			// console.log(shape.baseToolBar);
			
			for(var item in baseToolBar){
				shapeToolBar[baseToolBar[item]]=true;
				// console.log(item,baseToolBar[item],shapeToolBar);
			}
			
			for(var item in shape.toolBar){
				shapeToolBar[shape.toolBar[item]]=true;
				// console.log(item,baseToolBar[item],shapeToolBar);
			}
			
			vm.set('toolbar',shapeToolBar);
			
			// console.log('toolbar..');
			
			if (shape.type==='group') {
				theCanvas.isActiveObjectGroup= theCanvas.getActiveGroup()?'activeGroup':'groupObject';
			}else{
				theCanvas.isActiveObjectGroup='object';
			}
			// console.log('now select object is :  '+ theCanvas.isActiveObjectGroup ,'       ',shape);
			
			
			
			
			// alert(shape.prototype.type);
			
			vm.set('property',{});
			
			
			for(var i=0;i<baseProperty.length;i++){
				key=baseProperty[i];
				vm.set('property.'+key,shape.get(key));
			}
			
			// console.log(shape.property);
			
			if (theCanvas.isActiveObjectGroup==='object') {
				for(var i=0;i<shape.property.length;i++){
					key=shape.property[i];
					
					vm.set('property.'+key,shape.get(key));
				}
				
			}else if (theCanvas.isActiveObjectGroup==='activeGroup'){
				var firstShape=shape.getObjects()[0];
				var objectProperty=firstShape.property;
				shape.forEachObject(function(itemObject){
					objectProperty=Ext.Array.intersect(objectProperty,itemObject.property);
				});
				for(var i=0;i<objectProperty.length;i++){
					key=objectProperty[i];
					vm.set('property.'+key,firstShape.get(key));
				}
				// adjust to first object's property , x,y,width,height
				vm.set('property.type','group');
				vm.set('property.zIndex',-1);
				Ext.apply(vm.data.property,{
					// left:firstShape.get('left'),
					// top:firstShape.get('top'),
					width:firstShape.get('width'),
					height:firstShape.get('height'),
				});
				
				
				
				
			}else if (theCanvas.isActiveObjectGroup==='groupObject'){
				
			}
			
			// console.log(vm.get('property'));
			
			// vm.set('property.zIndex',shape.getZIndex());
			// vm.set('property.width',shape.getWidth());
			// vm.set('property.height',shape.getHeight());
			vm.set('property',vm.get('property'));
								 
		};
		
		var onMovingProperty= function (options) {
			var shape=options.target;
			shape.setCoords();
			theCanvas.renderAll();
			
			var vm=me.getViewModel();
			vm.set('property.top',options.target.getTop() );
			vm.set('property.left',options.target.getLeft() );
		};
		
		var onScalingProperty= function (options) {
			var shape=options.target;			
			
			var vm=me.getViewModel();
			Ext.apply(vm.data.property,{
					left: shape.get('left'),
					top: shape.get('top'),
					width: shape.get('width'),
					height: shape.get('height'),
					scaleX: shape.getScaleX() ,
					scaleY: shape.getScaleY() 
			});
			
			shape.setCoords();
			theCanvas.renderAll();
		};
		
		var onRotatingProperty= function (options) {
			var shape=options.target;			
			shape.setCoords();
			theCanvas.renderAll();
			
			var vm=me.getViewModel();
			vm.set('property.angle',options.target.angle);
			vm.set('property.top', options.target.getTop() );
			vm.set('property.left', options.target.getLeft() );
			
		}
		
		
		
		var ToolObject = fabric.util.createClass({
			
			cursor:'cross',
			
			initialize: function() {
			
			},
			
			onMouseUp:function(options) {
				theCanvas.activeTool='Pointer';		
				var shape=theCanvas.item(theCanvas.size()-1);				
				shape.setCoords();
				
				var activeGroup = theCanvas.getActiveGroup();

				if (activeGroup) {
					theCanvas.discardActiveGroup();
					theCanvas.setActiveObject(shape);	
				}

				theCanvas.renderAll();
				
			},
			
			setDefaultProperty:function(object,options){
				var pointer=theCanvas.getPointer(options.e);
				object.set({
					zIndex:theCanvas.size(),
					top : pointer.y, left : pointer.x, 
					stroke: theCanvas.stroke, strokeWidth: theCanvas.strokeWidth, fill : theCanvas.fill
				});
				
			},
			
			addNewObject:function(object) {
				// console.log(object.type);
				object.zIndex=theCanvas.size(),
				theCanvas.deactivateAll();
				theCanvas.add(object);
				theCanvas.setActiveObject(object);
			},
			onMouseMove: function(options) {
			}
		});
		
		
		
		var ToolPointer = fabric.util.createClass({
			initialize: function() {
			
			},
			onMouseDown: function(options) {
			},
			onMouseMove: function(options) {
			},
			onMouseUp: function(options) {
			}
		});
		
		var ToolRect = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				// var pointer=theCanvas.getPointer(options.e);
				var rect = new fabric.Rect({
					width : 1,	height : 1
				});
				this.setDefaultProperty(rect,options);
				// rect.set(cornerSize: 6,borderColor: 'red',cornerColor: '#006600',transparentCorners: false);
				
				// this.callSuper('addNewObject',rect);				
				this.addNewObject(rect);
			},
			onMouseMove: function(options) {
				
				// if(theCanvas.leftButtonPressed){
				if(options.e.buttons===1){
					theCanvas.setCursor('se-resize');
					var pointer=theCanvas.getPointer(options.e);
					var shape=theCanvas.item(theCanvas.size()-1);
					// console.log('circle mouse move');
					// console.log(shape.type);					
					// console.log(options.e.layerX+'    ,         '+options.e.layerY);		
					
					// shape.set({
						// width:pointer.x-shape.getLeft(),
						// height:pointer.y-shape.getTop()
					// });
					shape.MoveHandleTo(pointer);
				
					theCanvas.setActiveObject(shape);
				
					theCanvas.renderAll();
						
				}
			}
			// onMouseUp: function(options) {
				// this.callSuper('onMouseUp');
			// }
		});
		
		var ToolCircle = fabric.util.createClass(ToolRect, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				var pointer=theCanvas.getPointer(options.e);
				var circle = new fabric.Circle({
					radius: 1
				});
				this.setDefaultProperty(circle,options);
				this.addNewObject(circle);
				// console.log('circle mouse down');
			}			
		});
		var ToolTriangle = fabric.util.createClass(ToolRect, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				var pointer=theCanvas.getPointer(options.e);
				var triangle = new fabric.Triangle({  
					width: 1, height: 1
				});
				this.setDefaultProperty(triangle,options);
				this.addNewObject(triangle);
			}			
		});
		var ToolEllipse = fabric.util.createClass(ToolRect, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				var pointer=theCanvas.getPointer(options.e);
				var ellipse = new fabric.Ellipse({
						rx: 1,ry: 1,angle: 0
					});
				this.setDefaultProperty(ellipse,options);
				this.addNewObject(ellipse);	
				
			}			
		});	
		var ToolLine = fabric.util.createClass(ToolRect, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				var pointer=theCanvas.getPointer(options.e);
				var line = new fabric.Line([pointer.x,pointer.y, pointer.x+1,pointer.y+1], {
						fill: '#006600',
						stroke: '#FFCC00',strokeWidth: 2						
					});
				this.setDefaultProperty(line,options);
				this.addNewObject(line);		
			
			}			
		});	
		
		var ToolText = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var text = new fabric.Text('text',{
					zIndex:theCanvas.size(),
					left: pointer.x, top: pointer.y ,
					fontFamily: 'arial', fontSize: 40, fontStyle: 'italic',	fontWeight: 'normal',					
					fill:'#FFFF00', backgroundColor:'#006600',  textBackgroundColor: '#006600',
					textAlign: 'left',
					// textDecoration: 'underline',
					shadow: 'rgba(0,0,0,0.3) 5px 5px 5px',					
					stroke: '#FFFF00',	strokeWidth: 2			
				});
			this.addNewObject(text);
			}
			
		});	
		
		var Thermometer = fabric.util.createClass(fabric.Object, {

			type: 'Thermometer',
            
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				
				this.set('myValue', options.myValue || 20);
				this.set('start', options.start || 0);
				this.set('end', options.end || 100);
				// this.set('label', options.value || '');
				
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				// this.callSuper('_render', ctx);
				
				
				
				var x=-this.width/2,y=-this.height/2;
				var deltax=this.width/3,deltay=20;
				var newWidth=this.width-2*deltax,newHeight=this.height-2*deltay;
				
				ctx.font = '20px Helvetica';
				ctx.fillStyle = '#333';
				ctx.fillText(this.myValue, -x-10, -this.height/2 + 20);
				
				ctx.lineWidth = this.strokeWidth;
				ctx.strokeStyle = this.stroke;
				var p=new Path2D("m"+(x+deltax)+","+(y+deltay)+"l0,"+(newHeight-30)+"a"+this.width/3+","+this.width/3+" 0 1 0 "+newWidth+",0l0,-"+(newHeight-30)+"a"+newWidth/4+","+newWidth/2+" 90 0 0 -"+newWidth+",0z");
				ctx.stroke(p);
				
				var lineargradient = ctx.createLinearGradient(x, 0, -x, 0);
				// lineargradient.addColorStop(0, this.fill);
				lineargradient.addColorStop(0.1, 'white');
				lineargradient.addColorStop(1, this.fill);
				
				ctx.fillStyle=lineargradient;
				
				var origin=(this.myValue-this.start)*(newHeight-30)/(this.end-this.start);
				var str="m"+(x+deltax)+","+(y+deltay+newHeight-30-origin)+"l0,"+origin+"a"+this.width/3+","+this.width/3+" 0 1 0 "+newWidth+",0l0,-"+origin+"z";
				
				var currentP=new Path2D(str);
				ctx.fill(currentP);
				
				// console.log(this.width/3);
				
			}
		});
		
		ThermometerProperty=['start','end','myValue'];
		Thermometer.prototype.property=ThermometerProperty;	
		
		var ToolThermometer = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var thermometer = new Thermometer({
					zIndex:theCanvas.size(),
					left: pointer.x, top: pointer.y ,
					fill:'#ff0000',
					start:0,end:100,myValue:20,
					stroke: '#000000',	strokeWidth: 1,
					width:60,	height:180					
				});
			this.addNewObject(thermometer);
			}
			
		});	
		
		
		var Tank = fabric.util.createClass(fabric.Object, {

			type: 'Tank',
            
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				this.set('start', options.start || 0);
				this.set('end', options.end || 100);
				this.set('myValue', options.myValue || 20);
				this.set('innerColor', options.innerColor || '#00ff00');
				this.set('subType', options.subType || '1');
				
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				
				var x=-this.width/2,y=-this.height/2;
				var deltax=5,deltay=50,newWidth=this.width-2*deltax,newHeight=this.height-2*deltay;
				var origin=(this.myValue-this.start)*(newHeight*2/3)/(this.end-this.start);
				
				ctx.lineWidth = this.strokeWidth;
				ctx.strokeStyle = this.stroke;
				
				// var str="m-10,"+(40-origin)+"l0,"+origin+"a20,20 0 1 0 20,0l0,-"+origin+"z";
				
				var subTypeStr="m"+(x+deltax)+","+(y+deltay)+"l0,"+newHeight*2/3+"l"+newWidth/3+",40l0,"+newHeight/3+"l"+newWidth/3+",0l0,-"+newHeight/3+"l"+newWidth/3+",-40l0,-"+newHeight*2/3+"a"+newWidth/4+","+newWidth/2+" 90 0 0 -"+newWidth+",0z";
				// var subTypeStr1="m"+(x+20)+","+(y+40+90-origin)+"l0,"+origin+"l70,10l0,-"+(origin+20)+"z";
				var subTypeStr1="m"+(x+15)+","+(y+deltay+newHeight*2/3-origin)+"l0,"+origin+"l"+(newWidth-20)+",10l0,-"+(origin+20)+"z";
				// var subTypeStr2="m"+(x+48)+","+(y+35)+"l-20,30 l 0,15 l 10,5 l -5,20 l 20,30 l 30,-20l-5,-20l5,-10l0 ,-20z";
				var subTypeStr2="m"+(x+25)+","+(y+deltay)+"l-10,20 l 0,"+(newHeight*2/3-75)+" l 10,5 l -5,20 l 20,30 l "+(newWidth-50)+",-20l-5,-20l5,-10l0 ,-"+(newHeight*2/3-75)+"z";
				
				if (this.subType==='2'){
					
					origin=(this.myValue-this.start)*(newHeight)/(this.end-this.start);
					
					subTypeStr="m"+(x+deltax)+","+(y+deltay)+"l0,"+newHeight+"a"+newWidth/4+","+newWidth/2+" 90 0 0 "+newWidth+",0l0,-"+newHeight+"a"+newWidth/4+","+newWidth/2+" 90 0 0 -"+newWidth+",0z";
					
					subTypeStr1="m"+(x+15)+","+(y+deltay+newHeight-origin)+"l0,"+origin+"l"+(newWidth-20)+",10l0,-"+(origin+20)+"z";
					
					subTypeStr2="m"+(x+25)+","+(y+deltay)+"l-10,30 l 0,"+(newHeight-88)+" l 10,5 l -5,20 l 20,30 l "+(newWidth-50)+",-20l-5,-20l5,-10l0 ,-"+(newHeight-88)+"z";
					
				}
				
				// console.log(subTypeStr,this.subType);
				
				var lineargradient = ctx.createLinearGradient(x, 0, -x, 0);
				lineargradient.addColorStop(0, this.fill);
				lineargradient.addColorStop(0.5, 'white');
				lineargradient.addColorStop(1, this.fill);
				
				var p=new Path2D(subTypeStr);
				var p1=new Path2D(subTypeStr1);
				var p2=new Path2D(subTypeStr2);
				
				// p2.addPath(p);
				
				
				ctx.font = '20px Helvetica';
				ctx.fillStyle = '#333';
				ctx.fillText(this.myValue, x-30, -60);
				
				ctx.fillStyle='000000';
				ctx.fill(p2);
				
				ctx.globalCompositeOperation = 'source-atop';
				
				ctx.fillStyle=this.innerColor;		
				ctx.fill(p1);
				
				ctx.globalCompositeOperation = 'destination-over';
				
				ctx.stroke(p);
				ctx.fillStyle=lineargradient;				
				ctx.fill(p);
				
				ctx.globalCompositeOperation = 'source-over';
			}
		});
		
		TankProperty=['start','end','subType','innerColor','myValue'];
		Tank.prototype.property=TankProperty;	
		
		var ToolTank = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var tank = new Tank({
					zIndex:theCanvas.size(),
					subType:'1',innerColor:'#00ff00',
					left: pointer.x, top: pointer.y ,
					fill:'#A9A9A9',
					stroke: '#000000',	strokeWidth: 4,
					width:90,	height:280,
					start:0,end:100,myValue:20,					
				});
			this.addNewObject(tank);
			}
			
		});	
		//绘制圆
        var  myDrawCircle = function (ctx,x, y, r, color, lineWidth,fill) {
            ctx.lineWidth = lineWidth;
            ctx.strokeStyle = color;
			ctx.fillStyle = fill;
            ctx.beginPath();
            ctx.arc(x, y, r, 0, Math.PI * 2, true);
            ctx.closePath();
			if(color!=null){
				ctx.stroke();
			}
			if(fill!=null){
				ctx.fill();
			}
        };
        //绘制矩形
        var myDrawRectangle = function (ctx,x, y, w, h, color, lineWidth,fill,angle) {
            ctx.lineWidth = lineWidth;
            ctx.strokeStyle = color;
			ctx.fillStyle = fill;
			ctx.rotate(Math.PI / 180 * angle);
			if(color!=null){
				ctx.strokeRect(x, y, w, h);
			}
			if(fill!=null){
				ctx.fillRect(x, y, w, h);
			}
			// ctx.setTransform(1, 0, 0, 1, 0, 0);
        };
		var Lamp = fabric.util.createClass(fabric.Object, {

			type: 'Lamp',
            
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				
				this.set('myValue', options.myValue || true);
				this.set('subType', options.subType || '1');
				this.set('poleColor', options.poleColor || '#3CB371');
				this.set('myRadius', options.myRadius || Math.min(this.width,this.height)/2);
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				 
				var x=this.width/2,y=this.height/2;
				var deltax=5,deltay=5;
				 
				if (this.subType==='1'){
					
					var gradient = ctx.createRadialGradient(0,0,0,0,0,40);
					gradient.addColorStop(0,"#ffffff");
					gradient.addColorStop(1,this.fill);
					myDrawCircle(ctx,0,0,this.myRadius,this.stroke,this.strokeWidth,gradient);
					
					gradient = ctx.createRadialGradient(0,0,0,0,0,35);
					gradient.addColorStop(0,"#ffffff");
					gradient.addColorStop(1,this.poleColor);
					myDrawCircle(ctx,0,0,this.myRadius-5,this.stroke,this.strokeWidth,gradient);
					
					this.height=this.width=this.myRadius*2;
					
				}else if(this.subType==='2'){
					
				}
				
			}
		});
		
		LampProperty=['subType','myValue','poleColor','myRadius'];
		Lamp.prototype.property=LampProperty;	
		
		var ToolLamp = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var lamp = new Lamp({
					subType:'1',myValue:true,poleColor:'#3CB371',
					left: pointer.x, top: pointer.y ,
					fill:'#ffffff',
					stroke: '#000000',	strokeWidth: 4,
					width:100,	height:120					
				});
			this.addNewObject(lamp);
			}
			
		});	
		
		var Switch = fabric.util.createClass(fabric.Object, {

			type: 'Switch',
            
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				
				this.set('myValue', options.myValue || true);
				this.set('subType', options.subType || '1');
				this.set('poleColor', options.poleColor || '#3CB371');
				
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				 
				if (this.subType==='1'){
					ctx.font = '20px Helvetica';
					ctx.fillStyle = '#3CB371';
					ctx.fillText('Start', -65, 75);
					ctx.fillStyle = '#800000';
					ctx.fillText('Stop', 15, 75);
					var angle=35;
					if(!this.myValue){
						angle=-35;
					}
					// console.log(angle,this.myValue);
					myDrawCircle(ctx,0,0,40,this.stroke,this.strokeWidth,this.fill);
					myDrawRectangle(ctx,-5,-5,10,65,this.stroke,this.strokeWidth,this.poleColor,angle);
					myDrawCircle(ctx,0,0,2,'#FFFFF0',2,'#FF0000');
				
				}else if(this.subType==='2'){
					
				}

			}
		});
		
		SwitchProperty=['subType','myValue','poleColor'];
		Switch.prototype.property=SwitchProperty;	
		
		var ToolSwitch = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var mySwitch = new Switch({
					subType:'1',myValue:true,poleColor:'#3CB371',
					left: pointer.x, top: pointer.y ,
					fill:'#A9A9A9',
					stroke: '#000000',	strokeWidth: 4,
					width:100,	height:80					
				});
			this.addNewObject(mySwitch);
			}
			
		});	
		

		
		var myDrawArc = function (ctx,x, y, rx, ry, xRotation, largeArc,counterClockwise, ex,ey,color, lineWidth,fill) {
			ctx.lineWidth = lineWidth;
			ctx.strokeStyle = color;
			ctx.fillStyle = fill;
			
			var str='m'+x+','+y+'a'+rx+','+ry+' '+xRotation+' '+largeArc+' '+counterClockwise+' '+ex+','+ey+'z';
			console.log(str);
			
			var p=new Path2D(str);
			
			ctx.stroke(p);
			ctx.fill(p);
		};
		
		var myDrawShape = function (ctx,str,color, lineWidth,fill) {
			
			ctx.lineWidth = lineWidth;
			ctx.strokeStyle = color;
			ctx.fillStyle = fill;
			
			var p=new Path2D(str);
			if(color!=null){
				ctx.stroke(p);
			}
			if(fill!=null){
				ctx.fill(p);
			}
		};
		
		var myDrawPolygonLine = function (ctx,points, color, lineWidth,fill,angle,point) {
            ctx.lineWidth = lineWidth;
            ctx.strokeStyle = color;
			ctx.fillStyle = fill;
			if(typeof (point) != "undefined"){
				ctx.translate(point.x,point.y);
			}
			ctx.rotate(Math.PI / 180 * angle);
            ctx.beginPath();
            ctx.moveTo(points[0].x, points[0].y);
            for (var i = 1; i < points.length; i++) { 
                ctx.lineTo(points[i].x, points[i].y);
            }
			
			ctx.stroke();
            ctx.closePath();
			ctx.fill();
            
        };
		
		var myDrawLine = function (ctx,x0, y0, x1, y1, color, lineWidth) {
            ctx.lineWidth = lineWidth;
            ctx.strokeStyle = color;
            ctx.beginPath();
            ctx.moveTo(x0, y0);
            ctx.lineTo(x1, y1);
            // ctx.closePath();
            ctx.stroke();
        };
		// trapezoid
		var getTrapezoidPoints = function (x,y,topWidth,buttomWidth,height) {
			var delta=Math.floor((buttomWidth-topWidth)/2);
			return [{x:x,y:y},{x:buttomWidth,y:y},{x:delta+topWidth,y:-height},{x:delta,y:-height},{x:x,y:y}];
		}
		
		
		var Motor = fabric.util.createClass(fabric.Object, {

			type: 'Motor',
            
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				
				this.set('myValue', options.myValue || true);
				this.set('subType', options.subType || '1');
				this.set('tail', options.tail || false);
				this.set('lines', options.tail || true);
				this.set('diameter', options.diameter || Math.min(this.width,this.height)/4);
				this.set('pipeColor', options.pipeColor ||'#000000');
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				var str='';
				var lineargradient = ctx.createLinearGradient(0, -40, 0,40);
				lineargradient.addColorStop(0, this.fill);
				lineargradient.addColorStop(0.5, 'white');
				lineargradient.addColorStop(1, this.fill);
				
				if (this.subType==='1'){
					str="m-50,-25l100,0l8,10l0,30l-8,10l-100,0a50,50 90 0 1 0,-50z";

					myDrawShape(ctx,str,this.stroke,this.strokeWidth,lineargradient);
					
					myDrawRectangle(ctx,59,-5,15,10,this.stroke,this.strokeWidth,this.lineargradient,0);
					
					if(this.tail){
						myDrawRectangle(ctx,-73,-8,15,16,this.stroke,this.strokeWidth,this.lineargradient,0);
					}
					
					if(this.lines){
						ctx.beginPath();
						ctx.strokeStyle = '#A9A9A9';
					
						for(var i=0;i<6;i++){
							ctx.moveTo(-45,16-6*i);	
							ctx.lineTo(45, 16-6*i);
						}
					}
					
					ctx.stroke();
				}else if(this.subType==='2'){
					ctx.save();
					lineargradient = ctx.createLinearGradient(0, -40, 0,40);
					lineargradient.addColorStop(0, this.pipeColor);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.pipeColor);
					ctx.translate(-this.diameter*3/4,0);
					ctx.rotate(-Math.PI/2);
					this.height=this.width=this.diameter*4;
					str="m0,-"+this.diameter/2+"a"+this.diameter/2+","+this.diameter/4+" 90 0 0 0,"+this.diameter+"l"+this.diameter*7/4+",0l0,-"+this.diameter+"z";
					myDrawShape(ctx,str,this.stroke,this.strokeWidth,lineargradient);					
					myDrawRectangle(ctx,this.diameter*7/4,-this.diameter*5/8,this.diameter*1/4,this.diameter*10/8,this.stroke,this.strokeWidth,lineargradient,0);
					ctx.rotate(Math.PI/2);
					ctx.translate(this.diameter*3/4,0);
					ctx.fillStyle=this.fill;
					ctx.beginPath();
					ctx.arc (0,0 ,this.diameter*5/4,0,Math.PI*2,true) ;
					ctx.closePath();
					ctx.stroke();
					ctx.fill();
				
					
					this.height=this.width=this.diameter*4;
					str="m0,-"+this.diameter/2+"a"+this.diameter/2+","+this.diameter/4+" 90 0 0 0,"+this.diameter+"l"+this.diameter*7/4+",0l0,-"+this.diameter+"z";
					myDrawShape(ctx,str,this.stroke,this.strokeWidth,lineargradient);					
					myDrawRectangle(ctx,this.diameter*7/4,-this.diameter*5/8,this.diameter*1/4,this.diameter*10/8,this.stroke,this.strokeWidth,lineargradient,0);
					ctx.restore();
					
				}
			}
		});
		
		MotorProperty=['subType','myValue','lines','tail','diameter'];
		Motor.prototype.property=MotorProperty;	
		
		var ToolMotor = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var motor = new Motor({
					subType:'2',myValue:true,tail:false,lines:true,
					left: pointer.x, top: pointer.y ,
					fill:'#006400',
					stroke: '#000000',	strokeWidth: 2,
					width:200,	height:100					
				});
			this.addNewObject(motor);
			}
			
		});	//#ff0000

		var Fan = fabric.util.createClass(fabric.Object, {

			type: 'Fan',
            
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				
				this.set('myValue', options.myValue || true);
				this.set('subType', options.subType || '1');
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				 
				var x=this.width/2,y=this.height/2;
				var deltax=5,deltay=5,radius=Math.min(x-deltax,y-deltay);
				
				var gradient = ctx.createRadialGradient(0,0,90,0,0,120);
				gradient.addColorStop(0,"#ffffff");
				gradient.addColorStop(1,this.stroke);
				
				myDrawCircle(ctx,0,0,100,gradient,this.strokeWidth,'#ff0000');
				
				var str="m0,0c1,-32 -14,-67 -30,-60c-53,6 -58,39 -62,63c28,-17 68,-10 60,28l37,8l-5,-39z";

				myDrawShape(ctx,str,null,this.strokeWidth,this.fill);
				ctx.rotate(Math.PI / 180 * 120);
				// str="m0,0c1,-32 -14,-67 -30,-60c-53,6 -58,39 -62,63c28,-17 68,-10 60,28l37,8l-5,-39z";
				myDrawShape(ctx,str,null,this.strokeWidth,this.fill);
				ctx.rotate(Math.PI / 180 * 120);
				myDrawShape(ctx,str,null,this.strokeWidth,this.fill);
				
				this.originX='center';
				this.originY='center';
				
				// gradient = ctx.createRadialGradient(0,0,0,0,0,15);
				// gradient.addColorStop(0,"#ffffff");
				// gradient.addColorStop(1,this.fill);
				// myDrawCircle(ctx,0,0,15,null,this.strokeWidth,gradient);
			}
		});
		
		FanProperty=['subType','myValue'];
		Fan.prototype.property=FanProperty;	
		
		var ToolFan = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var fan = new Fan({
					subType:'1',myValue:true,
					left: pointer.x, top: pointer.y ,
					fill:'#A9A9A9',
					stroke: '#000000',	strokeWidth:25 ,
					width:200,	height:200					
				});
			this.addNewObject(fan);
			}
			
		});	

		var Pipe = fabric.util.createClass(fabric.Object, {

			type: 'Pipe',
            offset:0,
			
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				
				this.set('myValue', options.myValue || true);
				this.set('subType', options.subType || '1');
				this.set('diameter', options.diameter || this.height);
				this.set('innerLineColor', options.innerLineColor || '#0000ff');
				this.set('direction', options.direction || 0);
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				 
				var x=-this.width/2,y=-this.height/2;
				var str='';
				
				var lineargradient = ctx.createLinearGradient(0, -this.diameter/2, 0,this.diameter/2);
				lineargradient.addColorStop(0, this.fill);
				lineargradient.addColorStop(0.5, 'white');
				// lineargradient.addColorStop(0.75, 'white');
				lineargradient.addColorStop(1, this.fill);

				if (this.subType==='1'){
					this.height=this.width=this.diameter*2;
					str="m-"+(this.diameter)+",0l0,"+(this.diameter)+"l"+(this.width)+",0l-"+(this.diameter)+",-"+this.diameter+"z";
					
					lineargradient = ctx.createLinearGradient(0, 0, 0,this.diameter);
					lineargradient.addColorStop(0, this.fill);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.fill);
					myDrawShape(ctx,str,null,this.strokeWidth,lineargradient);
					
					str="m0,0l0,-"+(this.diameter)+"l"+this.diameter+",0l0,"+this.height+"z";
					lineargradient = ctx.createLinearGradient(0, 0, this.diameter,0);
					lineargradient.addColorStop(0, this.fill);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.fill);
					myDrawShape(ctx,str,null,this.strokeWidth,lineargradient);
					
				}else if(this.subType==='2'){
					
					// var offset = 5;
					// 13641071925 
					this.height=this.diameter;
					str="m"+x+","+y+"l0,"+(this.diameter)+"l"+this.width+",0l0"+",-"+(this.diameter)+"z";
					// console.log(str);
					myDrawShape(ctx,str,null,this.strokeWidth,lineargradient);
					
					// function draw() {
						ctx.setLineDash([10, 10]);
						ctx.lineDashOffset = -this.offset;
						// str="m"+x+","+(y+this.diameter/2)+"l"+this.width+",0";
						// myDrawShape(ctx,str,this.stroke,this.diameter/5,null);
						myDrawLine(ctx,x,y+this.diameter/2,x+this.width,y+this.diameter/2,this.innerLineColor,this.diameter/5);
					// }
						// ctx.restore();
						ctx.setLineDash([]);
					// setInterval(function march() {
					  // offset++;
					  // if (offset > 16) {
						// offset = 0;
					  // }
					  // draw();
					  // setTimeout(march, 20);
					// }
					
					// march();
					
					// str="m"+x+","+y+"l"+this.width+",0";
					// myDrawShape(ctx,str,this.stroke,this.strokeWidth,null);
					
					// str="m"+x+","+(y+this.diameter)+"l"+this.width+",0";
					// myDrawShape(ctx,str,this.stroke,this.strokeWidth,null);
					
					
				}else if(this.subType==='3'){
					this.height=this.width=this.diameter*3;
					var width=this.width/2,height=this.height/2;
					
					str="m-"+height+",-"+(this.diameter/2)+"l"+(this.diameter)+",0l"+(this.diameter/2)+","+(this.diameter/2)+"l-"+(this.diameter/2)+","+(this.diameter/2)+"l-"+(this.diameter)+",0z";
					
					myDrawShape(ctx,str,null,this.strokeWidth,lineargradient);
					
					lineargradient = ctx.createLinearGradient( -this.diameter/2, 0,this.diameter/2,0);
					lineargradient.addColorStop(0, this.fill);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.fill);
					
					str="m-"+(this.diameter/2)+",-"+height+"l0,"+(this.diameter)+"l"+(this.diameter/2)+","+(this.diameter/2)+"l-"+(this.diameter/2)+","+(this.diameter/2)+"l0,"+(this.diameter)+"l"+(this.diameter)+",0l0,-"+(this.height)+"z";
					
					myDrawShape(ctx,str,null,this.strokeWidth,lineargradient);
					
					
				}else if(this.subType==='4'){
					this.height=this.width=this.diameter*3;
					var width=this.width/2,height=this.height/2;
					
					str="m-"+width+",-"+(this.diameter/2)+"l"+(this.diameter)+",0l"+(this.diameter/2)+","+(this.diameter/2)+"l"+(this.diameter/2)+",-"+(this.diameter/2)+"l"+(this.diameter)+",0l0,"+(this.diameter)+"l-"+(this.diameter)+",0l-"+(this.diameter/2)+",-"+(this.diameter/2)+"l-"+(this.diameter/2)+","+(this.diameter/2)+"l-"+(this.diameter)+",0z";
					// console.log(str);
					myDrawShape(ctx,str,null,this.strokeWidth,lineargradient);
					
					lineargradient = ctx.createLinearGradient( -this.diameter/2, 0,this.diameter/2,0);
					lineargradient.addColorStop(0, this.fill);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.fill);
					
					str="m-"+(this.diameter/2)+",-"+height+"l0,"+(this.diameter)+"l"+(this.diameter/2)+","+(this.diameter/2)+"l-"+(this.diameter/2)+","+(this.diameter/2)+"l0,"+(this.diameter)+"l"+(this.diameter)+",0l0,-"+(this.diameter)+"l-"+(this.diameter/2)+",-"+(this.diameter/2)+"l"+(this.diameter/2)+",-"+(this.diameter/2)+"l0,-"+(this.diameter)+"z";
					
					myDrawShape(ctx,str,null,this.strokeWidth,lineargradient);
					
					
				}
				this.setCoords();
			}
		});
		
		PipeProperty=['subType','myValue','diameter','direction','innerLineColor'];
		Pipe.prototype.shapeScript={
			'offset':'function () {	if (offset > 16)  return  0; return offset++;	  }	'
		};
		Pipe.prototype.property=PipeProperty;	
		
		var ToolPipe = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var pipe = new Pipe({
					subType:'4',myValue:true,
					left: pointer.x, top: pointer.y ,
					fill:'#000000',
					stroke: '#000000',	strokeWidth:1 ,
					width:200,	height:25					
				});
			this.addNewObject(pipe);
			}
			
		});
		
		var Dashboard = fabric.util.createClass(fabric.Object, {

			type: 'Dashboard',
			
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				
				this.set('myValue', options.myValue || 45);
				this.set('subType', options.subType || '1');
				this.set('diameter', options.diameter || this.height);
				this.set('runColor', options.runColor || '#006400');
			
				this.set('startValue', options.startValue || 0);
				this.set('endValue', options.endValue || 100);
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				 
				var x=-this.width/2,y=-this.height/2;
				var str='';
				
				var lineargradient = ctx.createLinearGradient(0, -this.diameter/2, 0,this.diameter/2);
				lineargradient.addColorStop(0, this.fill);
				lineargradient.addColorStop(0.5, 'white');
				// lineargradient.addColorStop(0.75, 'white');
				lineargradient.addColorStop(1, this.fill);

				if (this.subType==='1'){
					var radius=this.diameter/2;
					this.height=this.width=this.diameter;
					ctx.save();
					ctx.translate(0,0);
					// ctx.scale(0.4,0.4);
					// ctx.rotate(Math.PI/2);
					
					ctx.fillStyle = '#333';
					ctx.strokeStyle = "black";					
					ctx.lineWidth = 2;
					ctx.lineCap = "round";

					ctx.save();
					// ctx.font = '15px Helvetica';
					ctx.rotate(90*Math.PI/180);
					for (var i=0;i<11;i++){
						ctx.beginPath();
						ctx.rotate(Math.PI*30/180);
						ctx.moveTo(radius,0);
						ctx.lineTo(radius*1.2,0);						
						// ctx.fillText(i, 80, 0);
						ctx.stroke();
					}
					ctx.restore();
					
					ctx.save();
					ctx.rotate(120*Math.PI/180);
					ctx.lineWidth = 1;
					for (i=0;i<50;i++){
					if (i%5!=0) {
					  ctx.beginPath();
					  ctx.moveTo(radius*1.05,0);
					  ctx.lineTo(radius*1.2,0);
					  ctx.stroke();
					}
					ctx.rotate(Math.PI/30);
					}
					ctx.restore();
					
					ctx.save();
					ctx.rotate(120*Math.PI/180);
					ctx.strokeStyle = "#D40000";
					ctx.lineWidth =2;
					ctx.beginPath();
					ctx.rotate(this.myValue*Math.PI/60);
					ctx.moveTo(-radius*0.1,0);
					ctx.lineTo(radius*0.9,0);
					ctx.stroke();
					ctx.arc(0,0,5,0,Math.PI*2,true);
					ctx.fillStyle = "#D40000";
					ctx.fill();
					ctx.restore();
					
					ctx.font = radius*0.3+'px Helvetica';
					ctx.fillText(this.myValue, -radius*0.2, radius*0.5);
					
					
					ctx.font = radius*0.2+'px Helvetica';
					ctx.fillStyle = '#0000ff';
					for (var i=0;i<11;i++){
						if(i<5)
							ctx.fillText(i*10, -(radius*0.9)*Math.cos((60-i*30)*Math.PI/180)-radius*0.05,(radius*0.8)*Math.sin((60-i*30)*Math.PI/180)+radius*0.05);
						else if(i===5)
							ctx.fillText(i*10, -(radius*0.9)*Math.cos((60-i*30)*Math.PI/180)-radius*0.1,(radius*0.8)*Math.sin((60-i*30)*Math.PI/180));
						else
							ctx.fillText(i*10, -(radius*0.9)*Math.cos((60-i*30)*Math.PI/180)-radius*0.1,(radius*0.8)*Math.sin((60-i*30)*Math.PI/180)+radius*0.05);
						console.log()
					}
					
					ctx.strokeStyle = "#D40000";	
					ctx.beginPath();
					ctx.arc(0,0,radius*1.2,120*Math.PI/180,420*Math.PI/180,false);
					ctx.stroke();
					
					ctx.restore();
					
					
				}else if(this.subType==='2'){
					this.height=this.diameter/2;
					this.width=this.height*2;
					ctx.save();
					ctx.translate(0,this.diameter*1/4);

					ctx.font = this.diameter*0.3+'px Helvetica';
					ctx.fillText(this.myValue, -this.diameter*0.16, 0);
					
					ctx.lineWidth=this.diameter*0.3;
					
					ctx.beginPath();
					ctx.strokeStyle = this.fill;	
					ctx.arc(0,0,this.diameter*1/2,Math.PI,Math.PI*2,false);
					ctx.stroke();
					
					ctx.beginPath();
					ctx.strokeStyle = this.runColor;	
					ctx.arc(0,0,this.diameter*1/2,Math.PI,Math.PI*(this.endValue-this.startValue+this.myValue)/(this.endValue-this.startValue),false);
					ctx.stroke();
					
					ctx.font = this.diameter*0.2+'px Helvetica';
					ctx.fillText(this.startValue, -this.diameter*0.9, 0);
					ctx.fillText(this.endValue, this.diameter*0.65, 0);
					
					ctx.restore();
					
				}else if(this.subType==='3'){
					
				}else if(this.subType==='4'){
					
					
				}
				this.setCoords();
			}
		});
		
		DashboardProperty=['subType','myValue','diameter','runColor','startValue','endValue'];
		Dashboard.prototype.property=DashboardProperty;	
		
		var ToolDashboard = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var dashboard = new Dashboard({
					subType:'2',myValue:45,
					left: pointer.x, top: pointer.y ,
					fill:'#A9A9A9',
					stroke: '#000000',	strokeWidth:1 ,
					width:100,	height:100					
				});
			this.addNewObject(dashboard);
			}
			
		});
		
		var Valve = fabric.util.createClass(fabric.Object, {

			type: 'Valve',
			
			initialize: function(options) {
				options || (options = { });
            
				this.callSuper('initialize', options);
				
				this.set('myValue', options.myValue || true);
				this.set('subType', options.subType || '1');
				this.set('valveColor', options.valveColor || '#0000ff');
				this.set('diameter', options.diameter || this.height);
			 },
            
			 // toObject: function() {
			// return fabric.util.object.extend(this.callSuper('toObject'), {
			  // label: this.get('label')
			// });
			 // },
            
			 _render: function(ctx) {
				 
				var x=-this.width/2,y=-this.height/2;
				var str='';
				
				var lineargradient = ctx.createLinearGradient(0, this.height/10, 0,this.height/2);
				lineargradient.addColorStop(0, this.fill);
				lineargradient.addColorStop(0.5, 'white');
				lineargradient.addColorStop(1, this.fill);

				if (this.subType==='1'){
					
					ctx.save();
					// ctx.translate(0,0);
					// ctx.scale(0.4,0.4);
					// ctx.rotate(Math.PI/2);
					
					ctx.fillStyle = '#333';
					ctx.strokeStyle = "black";					
					ctx.lineWidth = 2;
					// ctx.lineCap = "round";

					myDrawRectangle (ctx,x, this.height/10, this.width*5/8, this.height*2/5, null, this.strokeWidth,lineargradient,0); 
					myDrawRectangle (ctx,x+this.width/32, this.height/20, this.width*3/32, this.height*1/2, this.stroke ,this.strokeWidth,lineargradient,0) ;
					
					lineargradient = ctx.createLinearGradient( -this.width*3/8, 0,0,0);
					lineargradient.addColorStop(0, this.fill);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.fill);
					myDrawRectangle (ctx,-this.width*5/16, -this.height/10, this.width*4/16, this.height*1/5, null, this.strokeWidth,lineargradient,0) ;
					myDrawRectangle (ctx,-this.width*3/8, -this.height*3/10, this.width*6/16, this.height*1/5, null, this.strokeWidth,lineargradient,0) ;
					var str="m-"+this.width*3/32+",-"+(this.height*3/10)+"l"+(this.width*3/32)+",0l"+(this.width/16)+",-"+(this.height/20)+"l"+(this.width*7/16)+",0l0,-"+(this.height/10)+"l-"+(this.width*7/16)+",0z";
					
					myDrawShape(ctx,str,this.stroke,this.strokeWidth,'#006400');
					
					ctx.restore();
					
				}else if(this.subType==='2'){
					this.height=this.width=this.diameter*2;
					ctx.save();
					lineargradient = ctx.createLinearGradient( 0,0, 0,this.diameter);
					lineargradient.addColorStop(0, this.fill);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.fill);
					myDrawRectangle (ctx,x, 0, this.diameter*1/2, this.diameter*1/1, null, this.strokeWidth,lineargradient,0); 
					myDrawRectangle (ctx,this.diameter/2, 0, this.diameter*1/2, this.diameter*1/1, null ,this.strokeWidth,lineargradient,0) ;
					
					lineargradient = ctx.createLinearGradient( -this.diameter*1/2, 0,this.diameter*1/2,0);
					lineargradient.addColorStop(0, this.fill);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.fill);
					
					myDrawRectangle (ctx,-this.diameter/2, this.diameter*1/4, this.diameter*1/1, this.diameter*1/2, null ,this.strokeWidth,lineargradient,0) ;
					myDrawRectangle (ctx,-this.diameter/4, -this.diameter*1/4, this.diameter*1/2, this.diameter*2/4, null ,this.strokeWidth,lineargradient,0) ;
					lineargradient = ctx.createLinearGradient( -this.diameter*1/2, 0,this.diameter*1/2,0);
					lineargradient.addColorStop(0, this.valveColor);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.valveColor);
					myDrawRectangle (ctx,-this.diameter*4/8, -this.diameter*1/1, this.diameter*8/8, this.diameter*3/4, null ,this.strokeWidth,lineargradient,0) ;
					
					ctx.restore();
					
				}else if(this.subType==='3'){
					this.height=this.width=this.diameter*2;
					ctx.save();
					lineargradient = ctx.createLinearGradient( 0,0, 0,this.diameter);
					lineargradient.addColorStop(0, this.fill);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.fill);
					myDrawRectangle (ctx,x, 0, this.diameter*1/2, this.diameter*1/1, null, this.strokeWidth,lineargradient,0); 
					myDrawRectangle (ctx,this.diameter/2, 0, this.diameter*1/2, this.diameter*1/1, null ,this.strokeWidth,lineargradient,0) ;
					
					lineargradient = ctx.createLinearGradient( -this.diameter*1/2, 0,this.diameter*1/2,0);
					lineargradient.addColorStop(0, this.fill);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.fill);
					
					myDrawRectangle (ctx,-this.diameter/2, this.diameter*1/4, this.diameter*1/1, this.diameter*1/2, null ,this.strokeWidth,lineargradient,0) ;
					myDrawRectangle (ctx,-this.diameter/4, -this.diameter*1/4, this.diameter*1/2, this.diameter*2/4, null ,this.strokeWidth,lineargradient,0) ;
					lineargradient = ctx.createLinearGradient( -this.diameter*1/2, 0,this.diameter*1/2,0);
					lineargradient.addColorStop(0, this.valveColor);
					lineargradient.addColorStop(0.5, 'white');
					lineargradient.addColorStop(1, this.valveColor);
					ctx.fillStyle =lineargradient;
					ctx.beginPath();
					ctx.arc(0,-this.diameter*1/4,this.diameter*3/4,180*Math.PI/180,360*Math.PI/180,false);
					ctx.closePath();
					ctx.fill();
					ctx.restore();
					
				}else if(this.subType==='4'){
					
					
				}
				this.setCoords();
			}
		});
		
		ValveProperty=['subType','myValue','valveColor','diameter'];
		Valve.prototype.property=ValveProperty;	
		
		var ToolValve = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			onMouseDown: function(options) {
				
				var pointer=theCanvas.getPointer(options.e);
				var valve = new Valve({
					subType:'3',myValue:true,valveColor:'#006400',
					left: pointer.x, top: pointer.y ,
					fill:'#000000',
					stroke: '#000000',	strokeWidth:1 ,
					width:100,	height:50					
				});
			this.addNewObject(valve);
			}
			
		});
		
		var Hexagon = fabric.util.createClass(fabric.Polygon, {
			  type: 'Hexagon',

			  initialize: function(options) {
				options || (options = { });
				this.callSuper('initialize', options);
				this.set('sideCount', options.sideCount || 5);
			  },
			_render: function(ctx) {
				this.callSuper('_render', ctx);
			}
		});		
		
		HexagonProperty=['sideCount'];
		HexagonToolBar=['sideCount'];
		Hexagon.prototype.property=HexagonProperty;	
		Hexagon.prototype.toolBar=HexagonToolBar;	
		
		var ToolHexagon = fabric.util.createClass(ToolObject, {
			initialize: function() {
				
			},
			
			onMouseDown: function(options) {		
				var points=regularPolygonPoints(theCanvas.sideCount,1);
				var polygon = new Hexagon(points, {
					// stroke: 'red',
					// left: 50,
					// top: 50,
					// strokeWidth: 2,
					strokeLineJoin: 'round',
					sideCount:theCanvas.sideCount
				},false);
				this.setDefaultProperty(polygon,options);
				this.addNewObject(polygon);
			},
			
			onMouseMove: function(options) {
				if(options.e.buttons===1){
					theCanvas.setCursor('se-resize');
					
					var	shape=theCanvas.item(theCanvas.size()-1),
						oldLeft=shape.getLeft(),
						oldTop=shape.getTop();
					// console.log(oldLeft,'--------',oldTop);
					
					theCanvas.remove(shape);
					
					var	pointer=theCanvas.getPointer(options.e);
					var	width=pointer.x-oldLeft,
						height=pointer.y-oldTop,
						radius=Math.min(height/2,width/2),
						points=regularPolygonPoints(theCanvas.sideCount,radius);

						var	polygon = new Hexagon(points, {
							// zIndex:theCanvas.size(),
							// left:oldLeft,
							// top: oldTop,
							// fill:theCanvas.fill,
							// stroke: theCanvas.stroke,
							// strokeWidth: theCanvas.strokeWidth,
							// strokeLineJoin: 'round',
							// sideCount:theCanvas.sideCount
						},false);
						
						polygon.set({
							'zIndex':theCanvas.size(),
							'left':oldLeft,
							'top': oldTop,
							'fill':theCanvas.fill,
							'stroke': theCanvas.stroke,
							'strokeWidth': theCanvas.strokeWidth,
							'strokeLineJoin': 'round',
							'sideCount':theCanvas.sideCount
						}).setCoords();
						
					theCanvas.discardActiveObject(); 
					theCanvas.add(polygon);
					theCanvas.setActiveObject(polygon);				
					theCanvas.renderAll();
				}
			}
		
		});
		var StarPolygon = fabric.util.createClass(fabric.Polygon, {
			  type: 'StarPolygon',

			  initialize: function(options) {
				options || (options = { });
				this.callSuper('initialize', options);
				this.set('spikeCount', options.spikeCount || 5);
			  },
			_render: function(ctx) {
				this.callSuper('_render', ctx);
			}
		});

		StarPolygonProperty=['spikeCount'];
		StarPolygon.prototype.property=StarPolygonProperty;	
		
		StarPolygonToolBar=['spikeCount'];
		StarPolygon.prototype.toolBar=StarPolygonToolBar;	
		
		var ToolStarPolygon = fabric.util.createClass(ToolObject, {
			
			initialize: function() {
				
			},
			onMouseDown: function(options) {		
				
				var points=starPolygonPoints(theCanvas.spikeCount,2,1);
				var polygon = new StarPolygon(points, {
					// stroke: 'red',
					// left: 50,
					// top: 50,
					// strokeWidth: 2,
					strokeLineJoin: 'round',
					spikeCount:theCanvas.spikeCount
				},false);
				this.setDefaultProperty(polygon,options);
				this.addNewObject(polygon);
			},
			
			onMouseMove: function(options) {
				if(options.e.buttons===1){
					theCanvas.setCursor('se-resize');
					
					var	shape=theCanvas.item(theCanvas.size()-1),
						oldLeft=shape.getLeft(),
						oldTop=shape.getTop();
						
					theCanvas.remove(shape);
					
					var	pointer=theCanvas.getPointer(options.e),
						width=pointer.x-oldLeft,
						height=pointer.y-oldTop,
						radius=Math.min(height/2,width/2),
						points=starPolygonPoints(theCanvas.spikeCount,radius,radius/2),
						polygon = new StarPolygon(points, {
							// zIndex:theCanvas.size(),
							// left: oldLeft,
							// top: oldTop,
							// fill:theCanvas.fill,
							// stroke: theCanvas.stroke,
							// strokeWidth: theCanvas.strokeWidth,
							// strokeLineJoin: 'round'
						});
						
					polygon.set({
						'zIndex':theCanvas.size(),
						'left':oldLeft,
						'top': oldTop,
						'fill':theCanvas.fill,
						'stroke': theCanvas.stroke,
						'strokeWidth': theCanvas.strokeWidth,
						'strokeLineJoin': 'round',
						spikeCount:theCanvas.spikeCount
					}).setCoords();
					
					theCanvas.discardActiveObject(); 
					theCanvas.add(polygon);
					theCanvas.setActiveObject(polygon);				
					theCanvas.renderAll();
				}
			}
		
		});
		
		var Tools={
			// 'Polygon':new ToolPolygon,
			'Pointer':new ToolPointer,
			'Rect': new ToolRect,
			'Circle':new ToolCircle,
			'Triangle':new ToolTriangle,
			'Ellipse':new ToolEllipse,
			'Line':new ToolLine,
			'Text':new ToolText,
			'Hexagon':new ToolHexagon,
			'StarPolygon':new ToolStarPolygon,
			'Thermometer':new ToolThermometer,
			'Tank':new ToolTank,
			'Lamp':new ToolLamp,
			'Switch':new ToolSwitch,
			'Fan':new ToolFan,
			'Motor':new ToolMotor,
			'Pipe':new ToolPipe,
			'Valve':new ToolValve,
			'Dashboard':new ToolDashboard
			
		};
	
		
		var onCanvasMouseDown= function (options) {
			// theCanvas.leftButtonPressed=true;
			theCanvas.isActiveObjectGroup='blank';
			actionElement='canvas';
			var t=Tools[theCanvas.activeTool];			
			if (t!=null){
				t.onMouseDown(options);
			}
		};
		var onCanvasMouseMove= function (options) {

			var t=Tools[theCanvas.activeTool];
			if (t!=null){
				t.onMouseMove(options);
			}	
		};
		var onCanvasMouseUp= function (options) {
			// theCanvas.leftButtonPressed=false;
			
			var t=Tools[theCanvas.activeTool];
			if (t!=null){
				t.onMouseUp(options);
			}	
			// actionElement='idel';
		};
		
		
		theCanvas.on({
			
			'object:selected':onSelectProperty,
			'object:moving':  onMovingProperty,
			'object:scaling': onScalingProperty,
			// 'object:modified': onModifiedProperty,
			'object:rotating': onRotatingProperty,
				
			'mouse:down':onCanvasMouseDown,
			'mouse:move':  onCanvasMouseMove,
			'mouse:up': onCanvasMouseUp
		});
		
		for(var i=0;i<theCanvas.property.length;i++){
				key=theCanvas.property[i];
				// console.log(key,' -----> ',theCanvas[key]);
				mvvm.set('property.'+key,theCanvas[key]);
		} 
		
	},
	
	removeCavansObject:function(){
		var activeGroup = theCanvas.getActiveGroup();
		var activeObject = theCanvas.getActiveObject();
		
		if(activeGroup){
			activeGroup.forEachObject(function(activeObject){
				theCanvas.remove(activeObject)	;
			});
			theCanvas.discardActiveGroup();
		}else{
			if(activeObject){
				theCanvas.remove(activeObject)	;
			}
		}
			
		theCanvas.renderAll();
	},
	updateCavansObject:function( proterty , value){
			var activeGroup = theCanvas.getActiveGroup();
			var activeObject = theCanvas.getActiveObject();
			
			if(activeGroup){
				if(!Ext.Array.contains(['left','top'],proterty)){
					activeGroup.forEachObject(function(activeObject){
						theCanvas.item(activeObject.getZIndex()).set(proterty, value).setCoords();	
					});
				}else{
					activeGroup.set(proterty, value).setCoords();
				}
				// console.log('propertygrid : ',activeObject.getZIndex(),proterty,value);
			}else{
				if(activeObject){
					theCanvas.item(activeObject.getZIndex()).set(proterty, value).setCoords();	
					// console.log('propertygrid : ',activeObject.getZIndex(),proterty,value);
				}
			}
				
			theCanvas.renderAll();
	},
	onSetFillColor:function (picker) {
		
		var me=this;
		var value='#'+picker.getValue();
			// console.log('actionElement is '+actionElement);
		if(typeof theCanvas!='undefined'&&actionElement==='toolbar'){
			me.updateCavansObject('fill',value);
			theCanvas.fill=value;
			// console.log('color is '+value);
			
		}	
		
	},
	onSetStrokeColor:function (picker) {
		var me=this;
		var value='#'+picker.getValue();
		if(typeof theCanvas!='undefined'&&actionElement==='toolbar'){
			me.updateCavansObject('stroke',value);
			theCanvas.stroke=value;
		}	
		
	},
	onSpikeCountChange:function( newValue , oldValue , eOpts )  {
			// console.log('SpikeCount focus',actionElement,this.getViewModel().get('property'));
		if(typeof theCanvas!='undefined'&&actionElement==='toolbar'){
			theCanvas.spikeCount=newValue.value;
		// console.log('SpikeCount focus',newValue.value);
		}	
	},
	onSideCountChange:function ( newValue , oldValue , eOpts )  {
		// console.log('SideCount focus',actionElement,this.getViewModel().get('property'),theCanvas);
		if(typeof theCanvas!='undefined'&&actionElement==='toolbar'){
			// console.log('SpikeCount focus',newValue.value);
			theCanvas.sideCount=newValue.value;
			
		}	
	},
	onStrokeWidthChange:function( slider , newValue , thumb , eOpts )  {
		// console.log(newValue,theCanvas,actionElement);
		var me=this;
		var value=parseInt(newValue,10);
		if(typeof theCanvas!='undefined'&&actionElement==='toolbar'){
			me.updateCavansObject('strokeWidth',value);
			theCanvas.strokeWidth=value;
		}	
		
		// console.log('fill change');
	},
	
	onPropertychange:function ( source , recordId , value , oldValue , eOpts )  {
		actionElement='property';
		// console.log('actionElement is 1	'+actionElement);
		var vm=this.getViewModel();		
		vm.set('property',vm.get('property'));
		// // console.log('recordId:'+recordId+',value'+value+',oldValue'+oldValue+',source'+source['zIndex']);
		// this.updateCavansObject(source['zIndex'],recordId,value);
		// setTimeout("actionElement='idel'",500);
		// console.log('actionElement is 2	'+actionElement);
	},
	onPointerToolSelected:function () {
		theCanvas.activeTool='Pointer';	
		var needTools={};
		this.getViewModel().set('toolbar',needTools);
	},
	onRectToolSelected:function () {
		//theCanvas=30;
		//alert(theCanvas);
		// if (typeof theCanvas==='undefined')
			// theCanvas= new fabric.Canvas('canvas');
		//alert(this.theCanvas);
		// var rect = new fabric.Rect({top : 20,   left : 20,width : 60,height : 70,fill : "#006600",backgroundColor:'#006600',stroke: '#FFCC00',strokeWidth: 3,centeredRotation: true, originX: 'left', originY: 'top'});
		// canvas.hoverCursor = 'pointer';
		// theCanvas.add(rect);		
		theCanvas.activeTool='Rect';
		var needTools={stroke:true,strokeWidth:true,fill:true};
		this.getViewModel().set('toolbar',needTools);
	},
	
	onCircleToolSelected:function () {
		theCanvas.activeTool='Circle';	
		var needTools={stroke:true,strokeWidth:true,fill:true};
		this.getViewModel().set('toolbar',needTools);
	},
	
	onTextToolSelected:function () {

		theCanvas.activeTool='Text';
		var needTools={stroke:true,strokeWidth:true,fill:true};
		this.getViewModel().set('toolbar',needTools);
	},
	onHexagonToolSelected:function () {

		theCanvas.activeTool='Hexagon';
		var needTools={stroke:true,strokeWidth:true,fill:true,sideCount:true};
		this.getViewModel().set('toolbar',needTools);
		var sideCount=this.lookupReference('polygonSideCount');
		sideCount.setValue(theCanvas.sideCount);
	},
	
	onStarPolygonToolSelected:function () {

		theCanvas.activeTool='StarPolygon';
		var needTools={stroke:true,strokeWidth:true,fill:true,spikeCount:true};
		this.getViewModel().set('toolbar',needTools);
		var spikeCount=this.lookupReference('polygonSpikeCount');
		console.log(theCanvas.spikeCount);
		spikeCount.setValue(theCanvas.spikeCount);
		
		
	},
	onThermometerToolSelected:function () {

		theCanvas.activeTool='Thermometer';
	},
	onTankToolSelected:function () {

		theCanvas.activeTool='Tank';
	},
	onDashboardToolSelected:function () {

		theCanvas.activeTool='Dashboard';
	},
	onValveToolSelected:function () {

		theCanvas.activeTool='Valve';
	},
	onLampToolSelected:function () {

		theCanvas.activeTool='Lamp';
	},
	onSwitchToolSelected:function () {

		theCanvas.activeTool='Switch';
	},
	onPipeToolSelected:function () {

		theCanvas.activeTool='Pipe';
	},
	onFanToolSelected:function () {

		theCanvas.activeTool='Fan';
	},
	onMotorToolSelected:function () {

		theCanvas.activeTool='Motor';
	},
	onlineToolSelected:function () {		
		theCanvas.activeTool='Line';
		var needTools={stroke:true,strokeWidth:true,fill:true};
		this.getViewModel().set('toolbar',needTools);
	},
	onEllipseToolSelected:function () {		
		theCanvas.activeTool='Ellipse';
		var needTools={stroke:true,strokeWidth:true,fill:true};
		this.getViewModel().set('toolbar',needTools);
	},
	onTriangleToolSelected:function () {
		theCanvas.activeTool='Triangle';
		var needTools={stroke:true,strokeWidth:true,fill:true};
		this.getViewModel().set('toolbar',needTools);
	},
	onGroup:function () {
		var activeGroup = theCanvas.getActiveGroup();

		if (activeGroup) {
			var objectsInGroup = activeGroup.getObjects();

			theCanvas.discardActiveGroup();

			var objs = new Array();
			for (var obj in objectsInGroup) {
				objs.push(objectsInGroup[obj]);
				theCanvas.remove(objectsInGroup[obj]);
			}
			var group = new fabric.Group(objs);
			theCanvas.add(group);
			theCanvas.setActiveObject(group);
		}
	},
	onSendToBack:function () {		
		var activeObject = theCanvas.getActiveObject();
		// alert(activeObject.getZIndex());
		if (activeObject)
		{
			theCanvas.sendToBack(activeObject);			
			theCanvas.discardActiveObject();
		}		
		
	},
	onSendBackwards:function () {		
		var activeObject = theCanvas.getActiveObject();
		if (activeObject)
		{
			theCanvas.sendBackwards(activeObject,true);
			theCanvas.discardActiveObject();
		}		
		
	},
	
	onBringForward:function () {		
		var activeObject = theCanvas.getActiveObject();
		if (activeObject)
		{
			theCanvas.bringForward(activeObject,true);
			theCanvas.discardActiveObject();
		}		
		
	},
	onBringToFront:function () {		
		var activeObject = theCanvas.getActiveObject();
		if (activeObject)
		{
			theCanvas.bringToFront(activeObject);
			theCanvas.discardActiveObject();
		}		
		
	},
	onRotateLeft:function () {	
		var activeObject = theCanvas.getActiveObject();
		if (activeObject)
		{
			activeObject.centeredRotation=true;
			activeObject.angle-=90;
			if(activeObject.angle==-360) 
				activeObject.angle=0;
			activeObject.setCoords();
			theCanvas.discardActiveObject();
			
			theCanvas.setActiveObject(activeObject);
		}
	} ,

	canvasData:[],
	canvasDataScript:[],
	
	onScript:function () {	
		var text1=this.lookupReference('textfield1').value;
		var color1=this.lookupReference('textarea1').value;
		
		theCanvas.item(0).shapeScript={'text':text1};
		theCanvas.item(1).shapeScript={'fill':color1};
		// theCanvas.item(2).shapeScript={'width':10};
	
		// console.log(text1,color1);
	},
	onSaveCanvas:function () {	
		
		this.canvasData.splice(0,this.canvasData.length);
		this.canvasDataScript.splice(0,this.canvasDataScript.length);
			
		for(var i=0,length=theCanvas.size();i<length;i++){
			var script=theCanvas.item(i).shapeScript;
			console.log(theCanvas.item(i),script);
			if(Object.keys(script).length!=0){
				for(var key in script){
					// console.log(key,script[key]);
					var re=/\$[\d]+:[\d]+/g;
					var items=script[key].match(re);
					// var scriptContent=script[key].replace(re,'__value');
					var scriptContent=script[key];
					scriptContent='theCanvas.item('+i+').set(\''+key+'\','+scriptContent+').setCoords()';
					// console.log(items,scriptContent);
					if(items!=null){
						var newItems=Ext.Array.unique(items);
						for (var j=0,newItemsLength=newItems.length;j<newItemsLength;j++){
							var item=newItems[j].slice(1).split(':');
							// console.log(item,typeof this.canvasDataScript[0]);
							if(typeof this.canvasDataScript[item[0]]==='undefined'){
								// console.log('jjj');
								this.canvasDataScript[item[0]]={};
								this.canvasData[item[0]]={};
							}
							if( typeof this.canvasDataScript[item[0]]['p'+item[1]]==='undefined'){
								this.canvasDataScript[item[0]]['p'+item[1]]=[];
								this.canvasData[item[0]]['p'+item[1]]=0;
								// console.log(this.canvasDataScript[item[0]]);
							}
							this.canvasDataScript[item[0]]['p'+item[1]].push(scriptContent);
							// console.log(item,this.canvasDataScript[item[0]]);
						}
						
					}else{
						console.log(scriptContent);
						
						
					}
				}
			
			}
		}
		// console.log(this.canvasDataScript);
		// console.log(this.canvasData);
		
	},
	
	setCanvasData:function (no,newData){
		// console.log();
		var oldData=this.canvasData[no];
		var re=/\$[\d]+:[\d]+/g;
		
		for(var key in newData){
			var newValue=newData[key];
			if(newValue===oldData[key]){
				break;
			}
			oldData[key]=newValue;
			
			var script=this.canvasDataScript[no][key];
			// console.log(__value,script);
			for(var num=0;num<script.length;num++){
				
				var scriptContent=script[num].replace(re,newValue);
				// console.log(newData,scriptContent);
				
				// new Function(scriptContent);
				eval(scriptContent);
			}
			
		}
		theCanvas.renderAll();
	},
	
	onRun:function () {	
		var me=this;
		// var num = "1234 5678";
		// var newNum = num.replace(/(\d{4}) (\d{4})/,"$2 $1");
		// alert(newNum);
		// alert("baddad".match(/([bd]ad?)*/));
		// alert("mon and dad".match(/(mon( and dad)?)/g))
		// var re1 = /.*bbb/g;
		// alert(re1.exec("abbbaabbbaaabbbb1234")+"");
		// '<%13%><%2%><%3%>'.replace(/<%(\d+)%>/g,function(a,b,c,d){
			// console.log(a+'\t'+b+'\t'+c+'\t'+d);
			// return b;
			// alert("s$1:2 ad $1:4".match(/\$[\d]+:[\d]+/g));
		// });
		// var t={};
		// t.p0='hello';
		// console.log(t);
		// var i=0;
		// i=true;
		// alert(i);
		// 
		
		// var f=Ext.get('my-property-strokeWidthButton');
		// console.log(f);
		
		// var t=10;
		// setInterval(function(){
			// if(t<99)
              // me.setCanvasData(0,{p1:t=t+1});
			// else
				// t=0;
         // },500);	
		 
		var offset=0;
		setInterval(function(){
			theCanvas.item(0).set('offset',function () { offset++;	if (offset > 16) offset=0; return offset;	  }).setCoords();
			theCanvas.item(0)._render(theCanvas.getContext());
			theCanvas.renderAll();
			// console.log(offset);
         },20);
		 
			
		// console.log(this);
		// theCanvas.setActiveObject(theCanvas.item(0));
		// theCanvas.item(1).set('fill',	function(){  return 'red' ; } );
		// theCanvas.renderAll();
	}
});
