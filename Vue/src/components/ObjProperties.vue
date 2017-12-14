<template>
  <div>
    <v-tooltip right>
      <v-icon @click="dialog = true" style="cursor: pointer;" class="makeSmall" color="yellow" slot="activator">playlist_add</v-icon>
      <span>Add Props</span>
    </v-tooltip>
    <v-dialog v-model="dialog" scrollable max-width="300px">
      <v-card>
        <v-card-title>Select the object's properties to attach!</v-card-title>
        <v-divider></v-divider>
        <v-card-text style="height: 300px;">
          <v-checkbox label="GroupList" v-model="props" value="GroupList" hide-details color="white">></v-checkbox>
          <v-checkbox label="LayerName" v-model="props" value="LayerName" hide-details color="white">></v-checkbox>
          <v-checkbox label="ObjectColor" v-model="props" value="ObjectColor" hide-details color="white">></v-checkbox>
          <v-checkbox label="ObjectType" v-model="props" value="ObjectType" hide-details color="white">></v-checkbox>
          <v-checkbox label="IsValid" v-model="props" value="IsValid" hide-details color="white">></v-checkbox>
          <v-checkbox label="Document" v-model="props" value="Document" hide-details color="white">></v-checkbox>
          <v-checkbox label="IsDeletable" v-model="props" value="IsDeletable" hide-details color="white">></v-checkbox>
          <v-checkbox label="IsDeleted" v-model="props" value="IsDeleted" hide-details color="white">></v-checkbox>
          <v-checkbox label="IsNormal" v-model="props" value="IsNormal" hide-details color="white">></v-checkbox>
          <v-checkbox label="IsLocked" v-model="props" value="IsLocked" hide-details color="white">></v-checkbox>
          <v-checkbox label="IsHidden" v-model="props" value="IsHidden" hide-details color="white">></v-checkbox>
        </v-card-text>
        <v-card class="ma-2" color="grey darken-4">
          <v-card-text v-if="props.length > 0">
            <span v-if="props.length > 0">Selection: {{ props }}</span>
          </v-card-text>
        </v-card>
        <v-card-actions>
          <v-tooltip bottom>
            <v-btn @click.native="attachProps" slot="activator">
              <v-icon>play_for_work</v-icon>
            </v-btn>
            <span v-if="saveSchema">Attach properties to the tree<br> and save the template in mySpecs</span>
            <span v-if="!saveSchema">Attach properties to the tree<br> without saving the template</span>
          </v-tooltip>
          <v-tooltip bottom>
            <v-icon style="cursor: pointer;" flat icon v-if="saveSchema" @click="saveTheSchema" slot="activator">save</v-icon>
            <v-icon style="cursor: pointer;" flat icon v-if="!saveSchema" @click="saveTheSchema" slot="activator" color="grey darken-1">save</v-icon>
            <span v-if="saveSchema">Click to NOT save this template<br> in "mySpecs" when attaching</span>
            <span v-if="!saveSchema">Click to save this template<br> in "mySpecs" when attaching</span>
          </v-tooltip>
          <v-spacer></v-spacer>
          <v-btn flat icon color="red" @click.native="dialog = false">
            <v-icon>close</v-icon>
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
<script>
export default {
  components: {

  },

  data( ) {
    return {
      saveSchema: false,
      dialog: false,
      props: [ ],
    }
  },

  computed: {
    toPrint( ) {
      console.log( this.model.children )
    },
  },

  methods: {
    backToTree( ) {
      window.bus.$emit( 'change-to-treeTab', true );
    },

    saveTheSchema( ) {
      this.saveSchema = !this.saveSchema
    },

    attachProps( ) {
      this.dialog = false
      window.bus.$emit( 'add-properties', this.props)
      //window.bus.$emit( 'drop-props', this.props );
      this.$emit('addmykids')

    }

  }
}
</script>
<style>
</style>