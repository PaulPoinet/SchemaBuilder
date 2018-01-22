<template>
  <li>
    <div :class="{bold: isFolder}" id="myElement">
      <v-icon class="makeSmall" style="cursor: pointer;" color="grey lighten-2" v-if="isFolder" v-show="open" aria-hidden="true" @click="toggle" light>expand_more</v-icon>
      <v-icon class="makeSmall" style="cursor: pointer;" color="red" v-if="isFolder" v-show="!open" @click="toggle" light>chevron_right</v-icon>
      <v-icon class="makeSmall" v-if="!isFolder" light color="grey lighten-2">crop_free</v-icon>
      <v-icon class="makeSmall" v-if="isFolder && !open" light color="grey lighten-2">folder</v-icon>
      <v-icon class="makeSmall" v-if="isFolder && open" light color="grey lighten-2">folder_open</v-icon>
      <input v-if="editKey==true" type="text" font-face="Roboto" v-autowidth="{maxWidth: '960px', minWidth: '20px', comfortZone: 0}" @focus="$event.target.select()" id="input" v-model="objectKey" @blur="editKey=false" @keyup.enter="editKey=false">
      <label v-if="editKey==false && !lock" v-bind:title="messageEditLabel" @mouseover="isItObject" @mouseleave="hideThisObject" @dblclick="editTheKey"> {{objectKey}} </label>
      <label v-if="editKey==false && lock" class="lockColor" v-bind:title="messageCantEdit"> {{objectKey}} </label>
      <span v-if="!isFolder">:</span>
      <input v-if="editValue==true" type="text" font-face="Roboto" v-autowidth="{maxWidth: '960px', minWidth: '20px', comfortZone: 0}" @focus="$event.target.select()" id="input" v-model="objectValue" @blur="editValue=false" @keyup.enter="editValue=false">
      <label v-if="editValue==false && !isFolder && !lock" v-bind:title="messageEditLabel" @mouseover="isItObject" @mouseleave="hideThisObject" @dblclick="editTheValue"> {{objectValue}} </label>
      <label v-if="editValue==false && !isFolder && lock" class="lockColor" v-bind:title="messageCantEdit"> {{objectValue}} </label>
      <span v-if="isFolder && model.children.length > 0" v-show="!open" class="redFont">{...+{{model.children.length}}}</span>
      <span>&nbsp;</span>

      <v-tooltip right>
        <v-icon style="cursor: pointer;" @click="show" hover xs1 small flat color="grey lighten-2" class="makeSmall" slot="activator">more_vert</v-icon>
        <span>Options</span>
      </v-tooltip>
      <v-icon hover xs1 small flat color="grey lighten-1" v-if="this.objectKey=='Id' && this.showObjectsInTree==true && contains(selectedIds, this.objectValue)==true" class="makeSmall">add_box</v-icon>
      <v-menu transition="slide-x-transition" bottom right color="black" v-model="showMenu" absolute :position-x="x" :position-y="y">
        <v-list class="ma-0" dark dense>
          <v-list-tile>
            <v-tooltip right>
              <v-icon style="cursor: pointer;" color="grey lighten-2" class="makeSmall" v-if="!isFolder" @click="changeType" slot="activator">create_new_folder</v-icon>
              <span>Create child</span>
            </v-tooltip>
            <v-icon style="cursor: default;" color="grey darken-2" class="makeSmall" v-if="isFolder">create_new_folder</v-icon>
          </v-list-tile>
          <v-list-tile>
            <v-tooltip right>
              <v-icon style="cursor: pointer;" class="makeSmall" color="grey lighten-2" v-if="!lock" @click="changeBehaviour" slot="activator">lock_open</v-icon>
              <span>Lock</span>
            </v-tooltip>
            <v-tooltip right>
              <v-icon style="cursor: pointer;" class="makeSmall" color="grey lighten-2" v-if="lock" @click="changeBehaviour" slot="activator">lock</v-icon>
              <span>Unlock</span>
            </v-tooltip>
          </v-list-tile>
          <v-list-tile>
            <v-tooltip right>
              <v-icon style="cursor: pointer;" class="makeSmall" color="red" @click="trashThis" slot="activator">close</v-icon>
              <span>Delete</span>
            </v-tooltip>
          </v-list-tile>
          <v-list-tile>
            <obj-properties v-on:addmykids='addProperties'>
            </obj-properties>
          </v-list-tile>
          <v-list-tile>
            <v-tooltip right>
              <v-icon style="cursor: pointer;" class="makeSmall" color="blue" @click="nextTabEvent" slot="activator">widgets</v-icon>
              <span>Load Specs</span>
            </v-tooltip>
          </v-list-tile>
        </v-list>
      </v-menu>
      <span>&nbsp;</span>
      <span class="redFont" v-if="index>=0 && showIndices">({{index}})</span>
    </div>
    <ul v-sortable="{ onUpdate: onUpdate }" v-show="open" v-if="isFolder">
      <draggable v-model="model.children" :options="{group:'item', name: 'item', pull:true, sort: true}" @start="drag=true" @end="drag=false" class="drag">
        <item class="item" v-for="(model, index) in model.children" :model="model" :index='index' @mouseover="isItObject" @mouseleave="hideThisObject" @deleteMe='deleteKid'></item>
      </draggable>
      <div>

        <v-tooltip right>
          <v-icon style="cursor: pointer;" @click="showAddOptions" hover xs1 small flat color="grey lighten-1" class="makeSmall" slot="activator">add_to_photos</v-icon>
          <span>Add...</span>
        </v-tooltip>
        <v-menu transition="slide-x-transition" bottom right color="black" v-model="showAddMenu" absolute :position-x="x" :position-y="y">
          <v-list class="ma-0" dark dense>
            <v-list-tile>
              <v-tooltip right>
                <v-icon style="cursor: pointer;" class="makeSmall" color="grey lighten-1" slot="activator" @click='addSibling'>add_box</v-icon>
                <span>Add Item</span>
              </v-tooltip>
            </v-list-tile>
            <v-list-tile>
              <obj-properties v-on:addmykids='addProperties'>
              </obj-properties>
            </v-list-tile>
          </v-list>
        </v-menu>
      </div>
    </ul>
  </li>
</template>
<script>
import draggable from 'vuedraggable'
import ObjProperties from './ObjProperties.vue'
import ObjMenu from './ObjMenu.vue'
export default {
  name: 'item',
  Value: 'item',

  props: {
    model: Object,
    index: Number,
  },

  components: {
    draggable,
    ObjProperties,
    ObjMenu
  },

  data: function( ) {
    return {
      dropdown_font: [ 'Arial', 'Calibri', 'Courier', 'Verdana' ],
      Value: this.model.Key,
      messageDelete0: "Delete this object ",
      messageAdd0: "Create a child object",
      messageAdd1: "Add an object",
      messageEditLabel: "Double-click to edit",
      messageCantEdit: "Unlock to edit",
      messageLoadSpecs: "Plug existing specs",
      messageBuildSpecs: "Select object properties",
      objectKey: this.model.Key,
      objectValue: this.model.Value,
      open: true,
      editKey: false,
      editValue: false,
      lock: false,
      drag: true,
      showIndices: false,
      showMenu: false,
      showAddMenu: false,
      myProperties: [ ],
      x: 0,
      y: 0,
      customName: "myRhinoObject",
      myObjectId: null,
      showTheObjects: true,
      showTheGraph: true,
      objectIds: [ ],
      GlobalEdges: [ ],
      GlobalSiblingEdges: [],
      showObjectsInTree: false,
      selectedIds: [],

      //myRoot: null,
    }
  },
  computed: {
    isFolder( ) {
      return this.model.children && this.model.children.length
    },
  },



  methods: {
    onUpdate( ) {},

    show( e ) {
      e.preventDefault( )
      this.showMenu = false
      this.x = e.clientX
      this.y = e.clientY
      this.$nextTick( ( ) => {
        this.showMenu = true
      } )
    },

    showAddOptions( e ) {
      e.preventDefault( )
      this.showMenu = false
      this.x = e.clientX
      this.y = e.clientY
      this.$nextTick( ( ) => {
        this.showAddMenu = true
      } )
    },

    deleteKid( index ) {
      this.model.children.splice( index, 1 )
    },

    nextTabEvent( ) {
      window.bus.$emit( 'change-to-schemasTab', true )
    },
    trashThis( ) {
      this.$emit( 'deleteMe', this.index )
      if ( this.isFolder ) this.model.children = [ ]
    },

    toggle( ) {
      if ( this.isFolder ) {
        this.open = !this.open
      }
    },
    contains(a, obj) {
        for (var i = 0; i < a.length; i++) {
            if (a[i] === obj) {
                return true;
            }
        }
        return false;
    },
    unfoldAll( ) {
      if ( open ) {
        open = !open
      }
    },
    changeBehaviour( ) {
      this.lock = !this.lock
      this.drag = !this.drag
    },

    changeType( ) {
      if ( !this.isFolder ) {
        this.$set( this.model, 'children', [ ] )
        this.addChild( )
        this.open = true
      }
    },

    addChild( ) {
      //console.log('called add child')
      if ( !this.model.children ) this.$set( this.model, 'children', [ ] )
      this.model.children.push( {
        Key: "ObjectKey",
        Value: "ObjectValue",
      } )
    },


    addProperties( ) {
      if ( !this.model.children ) {
        this.$set( this.model, 'children', [ ] )
        this.model.children.push( { Key: 'Id', Value: this.myObjectId }, {
          Key: "RhinoProperties",
          Value: "example",
          children: [ ]

        } )
        for ( var i = 0; i < this.myProperties.length; i++ ) {
          this.model.children[ 1 ].children.push( {
            Key: this.myProperties[ i ].split( ' : ' )[ 0 ],
            Value: this.myProperties[ i ].split( ' : ' )[ 1 ],
          } )
        }
      } else {
        this.model.children.push( {
          Key: this.customName,
          Value: "myRhinoObject",
          children: [
            { Key: 'Id', Value: this.myObjectId },
            {
              Key: "RhinoProperties",
              Value: "example",
              children: [ ]
            }
          ]
        } )
        for ( var i = 0; i < this.myProperties.length; i++ ) {
          this.model.children[ this.model.children.length - 1 ].children[ 1 ].children.push( {
            Key: this.myProperties[ i ].split( ' : ' )[ 0 ],
            Value: this.myProperties[ i ].split( ' : ' )[ 1 ],
          } )
        }
      }
      // if(this.showTheObjects && this.model.children[ 0 ].Key == "Id"){
      //   Interop.showObject( this.model.children[ 0 ].Value)
      //}
      if ( this.showTheObjects ) {
        window.bus.$emit( 'show-me-wut-u-got' )
      }
      if ( this.showTheGraph ){
        window.bus.$emit( 'show-me-the-graph' )
      }
      window.bus.$emit('refresh-objects', true)
    },



    findParentAndNeighbors( myRoot, myValue ) {
      for ( var i = 0; i < myRoot.children.length; i++ ) {
        if ( myRoot.children[ i ].children && myRoot.children[ i ].children.length ) {
          if ( myRoot.children[ i ].children[ 0 ].Key == myValue ) {
            console.log( myRoot.children[ 0 ].key )
            console.log( myValue )
          }
          this.findParentAndNeighbors( myRoot.children[ i ] )
        } else {}

      }
    },

    isItObject( ) {
      //console.log(this.myRoot)
      if ( this.model.Value == "myRhinoObject" ) {
        var AllParentKidRels = [ ]
        for ( var i = 0; i < this.model.children.length; i++ ) {
          if ( this.model.children[ i ].Value == "myRhinoObject" ) {
            var myParentKidRel = [ ]
            myParentKidRel.push( this.model.children[ 0 ].Value )
            myParentKidRel.push( this.model.children[ i ].children[ 0 ].Value )
            AllParentKidRels.push( myParentKidRel )
          }
        }
        console.log( JSON.stringify( AllParentKidRels ) )
        Interop.showHoveredGraph( this.model.children[ 0 ].Value, JSON.stringify( AllParentKidRels ) )
      }
    },

    hideThisObject( ) {
      Interop.hideHoveredGraph( )
    },

    addSibling( ) {
      this.model.children.push( {
        Key: "ObjectKey",
        Value: "ObjectValue",
      } )
    },

    editTheKey( ) {
      this.editKey = !this.editKey
      this.$nextTick( function( ) {
        var object = document.getElementById( "input" );
        object.focus( );
      } );
    },

    editTheValue( ) {
      this.editValue = !this.editValue
      this.$nextTick( function( ) {
        var object = document.getElementById( "input" );
        object.focus( );
      } );
    },

    collectGUIDs( myModel ) {
      for ( var i = 0; i < myModel.children.length; i++ ) {
        if ( myModel.children[ i ].children && myModel.children[ i ].children.length ) {
          this.collectGUIDs( myModel.children[ i ] )
        } else if ( myModel.children[ i ].Key == "Id" ) {
          this.objectIds.push( myModel.children[ i ].Value )
        } else {}
      }
    },

    collectTheEdges( myModel ) {
      for ( var i = 0; i < myModel.children.length; i++ ) {
        if ( myModel.children[ i ].children && myModel.children[ i ].children.length ) {
          this.collectTheEdges( myModel.children[ i ] )
        } else if ( myModel.Value == "myRhinoObject" ) {

          var siblingEdges = []

          for ( var i = 0; i < myModel.children.length; i++ ) {
            if ( myModel.children[ i ].Value == "myRhinoObject" ) {
              siblingEdges.push(myModel.children[ i ].children[ 0 ].Value)
              var myParentKidRel = [ ]
              myParentKidRel.push( myModel.children[ 0 ].Value )
              myParentKidRel.push( myModel.children[ i ].children[ 0 ].Value )
              this.GlobalEdges.push( myParentKidRel )
              this.collectTheEdges( myModel.children[ i ] )
            }
          }

          if (siblingEdges.length > 1){
            this.GlobalSiblingEdges.push(siblingEdges)
          }           
        } else {}
      }
    },

    showMeTheObjects( ) {
      //if ( this.model.Value == "SchemaBuilder" ) {
      Interop.hideObjects( )
      this.objectIds = [ ]
      this.collectGUIDs( this.model )
      //console.log( this.objectIds )
      Interop.showObjects( JSON.stringify( this.objectIds ) )
    },

    showMeTheGraph(){
      Interop.hideEdges( ) // works that way...
      this.GlobalEdges = [ ]
      this.GlobalSiblingEdges = [ ]
      this.collectTheEdges( this.model )
      console.log(this.GlobalEdges)
      console.log(this.GlobalSiblingEdges)
      Interop.showEdges(JSON.stringify(this.GlobalEdges))
    }
  },

  mounted( ) {

    window.bus.$on( 'show-me-wut-u-got', state => {
      if ( this.showTheObjects && this.model.Value == "SchemaBuilder" ) {
        this.$nextTick( this.showMeTheObjects( ) )
      }
    } )
    window.bus.$on( 'show-me-the-graph', state => {
      if ( this.showTheGraph && this.model.Value == "SchemaBuilder" ) {
        this.$nextTick( this.showMeTheGraph( ) )
      }
    } )
    window.bus.$on( 'fold-global', state => {
      this.open = state
    } )
    window.bus.$on( 'unfold-global', state => {
      this.open = state
    } )
    window.bus.$on( 'show-global-indices', state => {
      this.showIndices = state
    } )
    window.bus.$on( 'add-properties', state => {
      this.myProperties = state
    } )
    window.bus.$on( 'obj-name', state => {
      this.customName = state
    } )
    window.bus.$on( 'obj-Id', state => {
      this.myObjectId = state
    } )
    window.bus.$on( 'show-global-objects', state => {
        this.showTheObjects = state
        if ( state == true && this.model.Value == "SchemaBuilder" ) {
          this.$nextTick( this.showMeTheObjects( ) )
        } else if ( state == false ) {
          Interop.hideObjects( )
        }
      })
    window.bus.$on( 'show-rel', state => {
        this.showTheGraph = state
        if ( state == true && this.model.Value == "SchemaBuilder" ) {
          this.$nextTick( this.showMeTheGraph( ) )
        } else if ( state == false ) {
          Interop.hideEdges( )
        }
      } )
   window.bus.$on( 'my-selected-ids', state => {
      this.showObjectsInTree = false;
      this.showObjectsInTree = true;
      this.selectedIds = JSON.parse( state );
      console.log(this.selectedIds)


    } )

  

},

}
</script>
<style>
ul {
  margin: 0 2px;
}

.indexFont {
  color: black;
  font-family: Menlo, Consolas, monospace;
  font-weight: lighter;
}

.item {
  padding: 0px;
  border-style: solid;
  border-width: 0px;
  border-radius: 6px;
  border-left-width: 2px;
  border-left-color: rgb(60, 60, 60);
}

.item:hover {
  padding: 0px;
  border-style: solid;
  border-width: 0px;
  border-radius: 6px;
  border-left-width: 2px;
  border-left-color: rgb(210, 210, 210);
}


.whiteFont {
  color: white;
}

.lockColor {
  color: rgb(100, 100, 100);
}

.redFont {
  color: rgb(255, 70, 70);
}

.makeSmall {
  font-size: 20px;
}
</style>