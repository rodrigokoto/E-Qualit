/*
|--------------------------------------------------------------------------
| Drag And Drop
|--------------------------------------------------------------------------
*/

APP.component.DragAndDrop = {

    dragSrcEl: {},

    bindDraggables: function() {
        var cols = document.querySelectorAll('.pai-calendario-processo, .calendar ul li');
        
        [].forEach.call(cols, function(col) {
        	col.addEventListener('dragstart', APP.component.DragAndDrop.drag, false);
        	col.addEventListener('dragenter', APP.component.DragAndDrop.handleDragEnter, false);
        	col.addEventListener('dragover', APP.component.DragAndDrop.handleDragOver, false);
        	col.addEventListener('dragleave', APP.component.DragAndDrop.handleDragLeave, false);
        	col.addEventListener('drop', APP.component.DragAndDrop.drop, false);
        	col.addEventListener('dragend', APP.component.DragAndDrop.handleDragEnd, false);
        });
    }, 
    
    createDraggables: function() {

    },

    allowDrop : function (ev) {
        ev.preventDefault();
    },

    drag: function (ev) {
        ev.dataTransfer.setData("text", ev.target.id);
    },

    drop : function (ev) {
        ev.preventDefault();
        var data = ev.dataTransfer.getData("text");
        ev.target.appendChild(document.getElementById(data));
    },



    handleDrop: function(e) {
    	if(e.stopPropagation){
    		e.stopPropagation();
	    }
		
    	if(APP.component.DragAndDrop.dragSrcEl != this){
	    	APP.component.DragAndDrop.dragSrcEl.innerHTML = this.innerHTML;
		    this.innerHTML = e.dataTransfer.getData('text/html');
    	}
    	return false;
    },
    
    handleDragEnd: function(e) {
        var cols = document.querySelectorAll('.pai-calendario-processo, .calendar ul li');
    	this.style.opacity = 1;
	    
        [].forEach.call(cols, function(col){
    		col.classList.remove('over');
    	});	
    },		
    
    handleDragEnter: function(e) {
    	this.classList.add('over');
    },
    
    handleDragLeave: function(e) {
    	this.classList.remove('over');
    },
    
    handleDragOver: function(e) {
    	if(e.preventDefault){
	    	e.preventDefault();
    	}

        e.dataTransfer.dropEffect = 'move';
	    return false;
    },
    
    handleDragStart: function(e) {
    	this.style.opacity = 0.4;
	    APP.component.DragAndDrop.dragSrcEl = this;
	    e.dataTransfer.effectAllowed = 'move';
	    e.dataTransfer.setData('text/html', this.innerHTML);
    }, 
    
    init : function() {
        var readyStateCheckInterval = setInterval(function() {
            if (document.readyState === "complete") {
                clearInterval(readyStateCheckInterval);
                //APP.component.DragAndDrop.createDraggables();
                //APP.component.DragAndDrop.bindDraggables();
            }
        }, 10);
    }

};