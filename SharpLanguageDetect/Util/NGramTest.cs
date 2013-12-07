namespace Frost.SharpLanguageDetect.Util {
    /*
     * @author Nakatani Shuyo
     *
     */
    public class NGramTest {

        /**
        * @throws java.lang.Exception
        */
        //@Before
        public void setUp() {
        }

        /**
        * @throws java.lang.Exception
        */
        //@After
        public void tearDown() {
        }

        /**
        * Test method for constants
        */
        //@Test
        public void testConstants() {
            //assertThat(NGram.N_GRAM,  is(3));
            //Assert.AreEqual(NGram.N_GRAM, 3);
        }

        /**
        * Test method for {@link NGram#normalize(char)} with Latin characters
        */
        //@Test
        public void testNormalizeWithLatin() {
            //Assert.AreEqual(NGram.normalize('\u0000'), ' ');
            //Assert.AreEqual(NGram.normalize('\u0009'), ' ');
            //Assert.AreEqual(NGram.normalize('\u0020'), ' ');
            //Assert.AreEqual(NGram.normalize('\u0030'), ' ');
            //Assert.AreEqual(NGram.normalize('\u0040'), ' ');
            //Assert.AreEqual(NGram.normalize('\u0041'), '\u0041');
            //Assert.AreEqual(NGram.normalize('\u005a'), '\u005a');
            //Assert.AreEqual(NGram.normalize('\u005b'), ' ');
            //Assert.AreEqual(NGram.normalize('\u0060'), ' ');
            //Assert.AreEqual(NGram.normalize('\u0061'), '\u0061');
            //Assert.AreEqual(NGram.normalize('\u007a'), '\u007a');
            //Assert.AreEqual(NGram.normalize('\u007b'), ' ');
            //Assert.AreEqual(NGram.normalize('\u007f'), ' ');
            //Assert.AreEqual(NGram.normalize('\u0080'), '\u0080');
            //Assert.AreEqual(NGram.normalize('\u00a0'), ' ');
            //Assert.AreEqual(NGram.normalize('\u00a1'), '\u00a1');
        }

        /**
        * Test method for {@link NGram#normalize(char)} with CJK Kanji characters
        */
        //@Test
        public void testNormalizeWithCJKKanji() {
            //Assert.AreEqual(NGram.normalize('\u4E00'), '\u4E00');
            //Assert.AreEqual(NGram.normalize('\u4E01'), '\u4E01');
            //Assert.AreEqual(NGram.normalize('\u4E02'), '\u4E02');
            //Assert.AreEqual(NGram.normalize('\u4E03'), '\u4E01');
            //Assert.AreEqual(NGram.normalize('\u4E04'), '\u4E04');
            //Assert.AreEqual(NGram.normalize('\u4E05'), '\u4E05');
            //Assert.AreEqual(NGram.normalize('\u4E06'), '\u4E06');
            //Assert.AreEqual(NGram.normalize('\u4E07'), '\u4E07');
            //Assert.AreEqual(NGram.normalize('\u4E08'), '\u4E08');
            //Assert.AreEqual(NGram.normalize('\u4E09'), '\u4E09');
            //Assert.AreEqual(NGram.normalize('\u4E10'), '\u4E10');
            //Assert.AreEqual(NGram.normalize('\u4E11'), '\u4E11');
            //Assert.AreEqual(NGram.normalize('\u4E12'), '\u4E12');
            //Assert.AreEqual(NGram.normalize('\u4E13'), '\u4E13');
            //Assert.AreEqual(NGram.normalize('\u4E14'), '\u4E14');
            //Assert.AreEqual(NGram.normalize('\u4E15'), '\u4E15');
            //Assert.AreEqual(NGram.normalize('\u4E1e'), '\u4E1e');
            //Assert.AreEqual(NGram.normalize('\u4E1f'), '\u4E1f');
            //Assert.AreEqual(NGram.normalize('\u4E20'), '\u4E20');
            //Assert.AreEqual(NGram.normalize('\u4E21'), '\u4E21');
            //Assert.AreEqual(NGram.normalize('\u4E22'), '\u4E22');
            //Assert.AreEqual(NGram.normalize('\u4E23'), '\u4E23');
            //Assert.AreEqual(NGram.normalize('\u4E24'), '\u4E13');
            //Assert.AreEqual(NGram.normalize('\u4E25'), '\u4E13');
            //Assert.AreEqual(NGram.normalize('\u4E30'), '\u4E30');
        }

        /**
        * Test method for {@link NGram#get(int)} and {@link NGram#addChar(char)}
        */
        //@Test
        public void testNGram() {
            NGram ngram = new NGram();
            //Assert.AreEqual(ngram.get(0), null);
            //Assert.AreEqual(ngram.get(1), null);
            //Assert.AreEqual(ngram.get(2), null);
            //Assert.AreEqual(ngram.get(3), null);
            //Assert.AreEqual(ngram.get(4), null);
            ngram.AddChar(' ');
            //Assert.AreEqual(ngram.get(1), null);
            //Assert.AreEqual(ngram.get(2), null);
            //Assert.AreEqual(ngram.get(3), null);
            ngram.AddChar('A');
            //Assert.AreEqual(ngram.get(1), "A");
            //Assert.AreEqual(ngram.get(2), " A");
            //Assert.AreEqual(ngram.get(3), null);
            ngram.AddChar('\u06cc');
            //Assert.AreEqual(ngram.get(1), "\u064a");
            //Assert.AreEqual(ngram.get(2), "A\u064a");
            //Assert.AreEqual(ngram.get(3), " A\u064a");
            ngram.AddChar('\u1ea0');
            //Assert.AreEqual(ngram.get(1), "\u1ec3");
            //Assert.AreEqual(ngram.get(2), "\u064a\u1ec3");
            //Assert.AreEqual(ngram.get(3), "A\u064a\u1ec3");
            ngram.AddChar('\u3044');
            //Assert.AreEqual(ngram.get(1), "\u3042");
            //Assert.AreEqual(ngram.get(2), "\u1ec3\u3042");
            //Assert.AreEqual(ngram.get(3), "\u064a\u1ec3\u3042");

            ngram.AddChar('\u30a4');
            //Assert.AreEqual(ngram.get(1), "\u30a2");
            //Assert.AreEqual(ngram.get(2), "\u3042\u30a2");
            //Assert.AreEqual(ngram.get(3), "\u1ec3\u3042\u30a2");
            ngram.AddChar('\u3106');
            //Assert.AreEqual(ngram.get(1), "\u3105");
            //Assert.AreEqual(ngram.get(2), "\u30a2\u3105");
            //Assert.AreEqual(ngram.get(3), "\u3042\u30a2\u3105");
            ngram.AddChar('\uac01');
            //Assert.AreEqual(ngram.get(1), "\uac00");
            //Assert.AreEqual(ngram.get(2), "\u3105\uac00");
            //Assert.AreEqual(ngram.get(3), "\u30a2\u3105\uac00");
            ngram.AddChar('\u2010');
            //Assert.AreEqual(ngram.get(1), null);
            //Assert.AreEqual(ngram.get(2), "\uac00 ");
            //Assert.AreEqual(ngram.get(3), "\u3105\uac00 ");

            ngram.AddChar('a');
            //Assert.AreEqual(ngram.get(1), "a");
            //Assert.AreEqual(ngram.get(2), " a");
            //Assert.AreEqual(ngram.get(3), null);

        }

    }

}