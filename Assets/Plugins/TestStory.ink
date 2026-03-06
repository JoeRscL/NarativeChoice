VAR ending_didapat = ""
VAR hari_ini = 1
VAR faith = 50
VAR wealth = 50

-> cek_hari

=== cek_hari ===
{
    - faith <= 0: -> game_over_faith
    - wealth <= 0: -> game_over_wealth
    - hari_ini == 1: -> day_1
    - hari_ini == 2: -> day_2
    - else: -> day_3
}

=== day_1 ===
Pria Misterius: "Mother... saya telah mencuri perhiasan untuk melunasi hutang. Saya pendosa."

* [Ampuni dia]
    ~ ending_didapat = "good_ending"
    ~ faith = faith + 15
    ~ wealth = wealth - 10
    Kamu: "Tuhan maha pengampun. Kembalikan barang itu."
    Pria Misterius: "Terima kasih, Mother."
    -> DONE

* [Hukum dia]
    ~ ending_didapat = "bad_ending"
    ~ faith = faith - 15
    ~ wealth = wealth + 15
    Kamu: "Pencurian adalah dosa besar. Serahkan dirimu."
    Pria Misterius: "Tolong cabut kata-katamu!"
    -> DONE

=== day_2 ===
Seorang wanita bangsawan berlutut dengan gemetar.
Wanita Bangsawan: "Mother, saya telah meracuni suami saya demi harta..."

* [Berikan pengampunan]
    ~ ending_didapat = "good_ending"
    ~ faith = faith + 20
    ~ wealth = wealth - 15
    Kamu: "Pertobatan yang tulus akan menghapus dosamu."
    Wanita Bangsawan: "Terima kasih atas kebaikan Anda."
    -> DONE

* [Kutuk perbuatannya]
    ~ ending_didapat = "bad_ending"
    ~ faith = faith - 20
    ~ wealth = wealth + 20
    Kamu: "Dosamu terlalu berat. Keadilan dunia harus ditegakkan."
    Wanita Bangsawan: "Tidak! Anda tidak mengerti penderitaanku!"
    -> DONE

=== day_3 ===
Seorang ksatria masuk dengan zirah berlumuran darah.
Ksatria: "Saya membunuh penduduk desa yang tidak bersalah atas perintah raja."

* [Tenangkan jiwanya]
    ~ ending_didapat = "good_ending"
    ~ faith = faith + 20
    ~ wealth = wealth - 20
    Kamu: "Tuhan melihat penyesalanmu."
    Ksatria: "Saya akan mengabdikan sisa hidup saya untuk menebus ini."
    -> DONE

* [Tolak pengakuannya]
    ~ ending_didapat = "bad_ending"
    ~ faith = faith - 20
    ~ wealth = wealth + 10
    Kamu: "Darah di tanganmu tidak bisa dibasuh hanya dengan kata-kata."
    Ksatria: "Kalau begitu tidak ada harapan bagiku."
    -> DONE

=== game_over_faith ===
~ ending_didapat = "game_over"
Kepercayaan umat telah hancur. Gereja ditinggalkan dan kamu diusir karena dianggap sesat.
* [Tamat]
    -> DONE

=== game_over_wealth ===
~ ending_didapat = "game_over"
Gereja kehabisan dana. Kamu tidak bisa lagi menghidupi para suster dan terpaksa menutup pintu gereja selamanya.
* [Tamat]
    -> DONE