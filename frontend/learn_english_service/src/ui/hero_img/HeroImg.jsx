import styles from '../hero_img/hero_img.module.css';

function HeroImg(){
    return(
        <div className={styles.hero_img}>
            <img src="/hero/hero.png" alt=""/>
        </div>
    )
}

export default HeroImg;