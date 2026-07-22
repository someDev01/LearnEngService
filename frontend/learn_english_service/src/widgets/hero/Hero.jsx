import styles from '../hero/hero.module.css';
import PreviewHero from '../preview_hero/PreviewHero';
import Opportunites from './../opportunites/Opportunies';

function Hero(){

    return(
        <section className={styles.hero}>
            <PreviewHero/>
            <Opportunites/>
        </section>
    )
}

export default Hero;