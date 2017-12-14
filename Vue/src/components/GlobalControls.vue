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
      <!--<v-tooltip bottom>
        <v-btn class="ma-1" medium color="grey darken-2" @click="showIndices" slot="activator">
          <v-icon light>info_outline</v-icon>
        </v-btn>
        <span>Show/hide indices</span>
      </v-tooltip>-->
    </div>
    <br>
    <br />
    <ul>
    <smart-item :model="tree"></smart-item>
    </ul>
  </div>
</template>
<script>
import SmartItem from './SmartItem.vue'
import Data from './Data.js'

export default {
  components: {
    SmartItem,
  },

  data( ) {
    return {
      tree: Data,
      showIndicesG: false,
    }
  },


  methods: {
    foldGlobalToggle( ) {
      window.bus.$emit( 'fold-global', false );
    },
    unfoldGlobalToggle( ) {
      window.bus.$emit( 'unfold-global', true );
    },
    showIndices( ) {
      this.showIndicesG = !this.showIndicesG,
        window.bus.$emit( 'show-global-indices', this.showIndicesG );
    }
  }
}
</script>
<style>
ul {
  padding-left: 1em;
  line-height: 1.5em;
}
</style>