import styles from '../hero/hero.module.css';
import Opportunites from '../../ui/opportunites/Opportunies';
import PreviewHero from '../preview_hero/PreviewHero';

function Hero(){

    return(
        <section className={styles.hero}>
            <PreviewHero/>
            <Opportunites/>
        </section>
    )
}

export default Hero;