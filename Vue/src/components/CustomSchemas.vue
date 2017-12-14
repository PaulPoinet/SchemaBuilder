<template>
  <v-layout column>
    <v-flex xs12 sm6 offset-sm3>
      <v-container fluid grid-list-md>
        <v-layout row wrap>
          <v-flex v-bind="{ [`xs${card.flex}`]: true }" v-for="card in cards" :key="card.title">
            <v-card>

                <v-container fill-height fluid>
                  <v-layout fill-height>
                    <v-flex xs12 align-end flexbox>
                      <span class="headline white--text" v-text="card.title"></span>
                    </v-flex>
                  </v-layout>
                </v-container>

              <v-card-actions >
                <v-spacer></v-spacer>
                <v-btn icon>
                  <v-icon>favorite</v-icon>
                </v-btn>
                <v-btn icon>
                  <v-icon>bookmark</v-icon>
                </v-btn>
                <v-btn icon>
                  <v-icon>share</v-icon>
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-flex>
        </v-layout>
      </v-container>
    </v-flex>
  </v-layout>
</template>
<script>



export default {
  components: {},

  data: ( ) => ( {
    cards: [
      { title: 'Spec1', flex: 6 },
      { title: 'Spec2', flex: 6 },
      { title: 'Spec3', flex: 6 }
    ]
  } ),

  methods: {
    backToTree( ) {
      window.bus.$emit( 'change-to-treeTab', true );
    },
  }
}
</script>
<style>
</style>