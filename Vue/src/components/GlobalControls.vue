<template>
  <div>
    <div class="text-xs-center">
      <v-tooltip bottom>
        <v-btn class="ma-1" medium color="red" @click="foldGlobalToggle" slot="activator">
          <v-icon light>chevron_right</v-icon>
        </v-btn>
        <span>Collapse all</span>
      </v-tooltip>
      <v-tooltip bottom>
        <v-btn class="ma-1" medium color="grey darken-2" @click="unfoldGlobalToggle" slot="activator">
          <v-icon light>expand_more</v-icon>
        </v-btn>
        <span>Expand all</span>
      </v-tooltip>
      <v-tooltip bottom>
       
          <v-icon class="ma-3" style="cursor: pointer;" light color="grey darken-1" v-if="!showObjectsG" @click="showObjects" slot="activator">fa-cubes</v-icon>
          <v-icon class="ma-3" style="cursor: pointer;" light color="white" v-if="showObjectsG" @click="showObjects" slot="activator">fa-cubes</v-icon>
       
        <span>Show/Hide</span>
      </v-tooltip>
      <v-tooltip bottom>
    
          <v-icon style="cursor: pointer;" light color="grey darken-1" v-if="!showRel" @click="showRelationship" slot="activator">share</v-icon>
          <v-icon style="cursor: pointer;" light color="white" v-if="showRel" @click="showRelationship" slot="activator">share</v-icon>
        
        <span>Show/Hide</span>
      </v-tooltip>
      <!--<v-tooltip bottom>
        <v-btn class="ma-1" medium color="grey darken-2" @click="showIndices" slot="activator">
          <v-icon light>info_outline</v-icon>
        </v-btn>
        <span>Show/Hide indices</span>
      </v-tooltip>-->
    </div>
    <br>
    <br />
  <v-card class="ma-3">
    <v-card-text>
      <v-slider  label="Spread Objects" v-model="value1" step="0" color="red" append-icon="filter_center_focus"></v-slider>
    </v-card-text>
  </v-card>
    <ul>
      <smart-item :model="tree"></smart-item>
    <compiling-menu>
    </compiling-menu>
    </ul>



  </div>
</template>
<script>
import SmartItem from './SmartItem.vue'
import Data from './Data.js'
import CompilingMenu from './CompilingMenu.vue'
export default {
  components: {
    SmartItem,
    CompilingMenu
  },

  data( ) {
    return {
      tree: Data,
      showIndicesG: false,
      showObjectsG: true,
      showRel: true,
      value1: 0,
    }
  },


  methods: {
    foldGlobalToggle( ) {
      window.bus.$emit( 'fold-global', false );
      console.log(this.value1)
    },
    unfoldGlobalToggle( ) {
      window.bus.$emit( 'unfold-global', true );
    },
    showIndices( ) {
      this.showIndicesG = !this.showIndicesG,
        window.bus.$emit( 'show-global-indices', this.showIndicesG );
    },
    showObjects( ) {
      this.showObjectsG = !this.showObjectsG
      console.log(this.showObjectsG)
      window.bus.$emit( 'show-global-objects', this.showObjectsG );
    },
    showRelationship(){
      this.showRel = !this.showRel
      window.bus.$emit( 'show-rel', this.showRel );
    }
  },
  mounted(){
    window.bus.$on( 'refresh-objects', state => { 
      this.$nextTick(Interop.spreadObjects(this.value1, state))
    } )
  },
  updated( ) {
    //console.log(this.value1)

    Interop.spreadObjects(this.value1, false)


  },


}
</script>
<style>
ul {
  padding-left: 1em;
  line-height: 1.5em;
}
</style>